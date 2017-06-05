var selectedDepartmentId = "00000000-0000-0000-0000-000000000000";
$(function () {
    $("#btnAddRoot").click(function () { add(0); });
    $("#btnAdd").click(function () { add(1); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $("#btnEdit").click(function () { edit(); });
    $("#btnLoadRoot").click(function () {
        selectedDepartmentId = "00000000-0000-0000-0000-000000000000";
        loadTables();
    });
    initTree();
});

function initTree() {
    $.jstree.destroy();
    $.ajax({
        type: "Get",
        url: "/Department/GetDepartmentTreeData?_t=" + new Date().getTime(),    //获取数据的ajax请求地址
        success: function (data) {
            $('#treeDiv').jstree({       //创建JsTtree
                'core': {
                    'data': data,        //绑定JsTree数据
                    "multiple": false    //是否多选
                },
                "plugins": ["state", "types", "wholerow"]  //配置信息
            })
            $("#treeDiv").on("ready.jstree", function (e, data) {   //树创建完成事件
                data.instance.open_all();    //展开所有节点
            });
            $("#treeDiv").on('changed.jstree', function (e, data) {   //选中节点改变事件
                var node = data.instance.get_node(data.selected[0]);  //获取选中的节点
                if (node) {
                    selectedDepartmentId = node.id;
                    loadTables();
                };
            });
        }
    });

}

var $table = $('#tb_data');
var oTable;
function loadTables() {
    if (oTable == null) {
        oTable = new TableInit();
        oTable.Init();
    } else {
        $table.bootstrapTable("refresh");
    }
}

var TableInit = function () {
    var oTableInit = new Object();
    oTableInit.queryParams = function (params) {
        var temp = {
            StartPage: params.offset / params.limit + 1,
            PageSize: params.limit,
            Order: params.order,
            Sort: params.sort,
            SearchFelds: [
                {
                    Field: "parentId",
                    Val: selectedDepartmentId
                }
            ]
        };
        return temp;
    };
    oTableInit.Init = function () {
        $table.bootstrapTable({
            url: "/Department/GetDepartmentsByParent",
            method: "post",
            striped: true,
            pagination: true,
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            queryParams: oTableInit.queryParams,
            clickToSelect: true,
            columns: [{
                checkbox: true
            }, {
                field: 'code',
                title: '编码',
                align: "center"
            }, {
                field: "name",
                title: "名称"
            }, {
                field: "manager",
                title: "负责人"
            }, {
                field: "contactNumber",
                title: "负责人电话"
            }, {
                field: "createTime",
                title: "创建时间",
                formatter: function (value) {
                    return moment(value).format("YYYY-MM-DD HH:mm:ss ");
                }
            }, {
                field: "reamrks",
                title: "备注"
            }]
        });
    };
    return oTableInit;
};

//新增
function add(type) {
    if (type === 1) {
        if (selectedDepartmentId === "00000000-0000-0000-0000-000000000000") {
            layer.alert("请选择父部门。");
            return;
        }
        $("#ParentId").val(selectedDepartmentId);
    }
    else {
        $("#ParentId").val("00000000-0000-0000-0000-000000000000");
    }
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#Code").val("");
    $("#Name").val("");
    $("#Manager").val("");
    $("#ContactNumber").val("");
    $("#Reamrks").val("");
    $("#Title").text("新增顶级");
    //弹出新增窗体
    $("#addRootModal").modal("show");
};

//保存数据
function save() {
    $.ajax({
        type: "Post",
        url: "/Department/Edit",
        data: $("#editForm").serialize(),
        success: function (data) {
            if (data.result == "Success") {
                initTree();
                $("#addRootModal").modal("hide");
            } else {
                layer.tips(data.message, "#btnSave");
            };
        }
    });
};

//编辑
function edit() {
    var rows = $table.bootstrapTable("getSelections");
    if (rows.length == 0) {
        layer.alert("请选中需要编辑的数据。");
        return;
    }
    if (rows.length > 1) {
        layer.msg("你选中的多行，默认取你选中的第一行进行编辑。");
    }
    $.ajax({
        type: "Post",
        url: "/Department/Get",
        data: { id: rows[0].id },
        success: function (data) {
            $("#Id").val(data.id);
            $("#ParentId").val(data.parentId);
            $("#Name").val(data.name);
            $("#Code").val(data.code);
            $("#Manager").val(data.manager);
            $("#ContactNumber").val(data.contactNumber);
            $("#Remarks").val(data.remarks);
            $("#Title").text("编辑功能")
            $("#addRootModal").modal("show");
        }
    })
};

//删除
function deleteMulti() {
    var ids = "";
    var rows = $table.bootstrapTable("getSelections");
    for (var i = 0; i < rows.length; i++) {
        ids += rows[i].id + ",";
    }
    ids = ids.substring(0, ids.length - 1);
    if (ids.length == 0) {
        layer.alert("请选择要删除的记录。");
        return;
    };
    //询问框
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        var sendData = { "ids": ids };
        $.ajax({
            type: "Post",
            url: "/Department/DeleteMuti",
            data: sendData,
            success: function (data) {
                if (data.result == "Success") {
                    initTree();
                    layer.closeAll();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        });
    });
};