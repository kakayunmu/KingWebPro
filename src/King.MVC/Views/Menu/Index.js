var selectedMenuId = "00000000-0000-0000-0000-000000000000";

$(function () {
    $("#btnAddRoot").click(function () { add(0); });
    $("#btnAdd").click(function () { add(1); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $("#btnEdit").click(function () { edit(); });
    $("#btnLoadRoot").click(function () {
        selectedMenuId = "00000000-0000-0000-0000-000000000000";
        loadTables();
    });
    initTree();
});
//加载功能树
function initTree() {
    $.jstree.destroy();
    $.ajax({
        type: "Get",
        url: "/Menu/GetMenuTreeData?_t=" + new Date().getTime(),    //获取数据的ajax请求地址
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
                    selectedMenuId = node.id;
                    loadTables();
                };
            });
        }
    });

}
var $table = $('#tb_data');
var oTable;
//加载功能列表数据
function loadTables() {
    if (oTable == null) {
        oTable = new TableInit();
        oTable.Init();
    } else {
        $table.bootstrapTable("refresh");
    }
}

//新增
function add(type) {
    if (type === 1) {
        if (selectedMenuId === "00000000-0000-0000-0000-000000000000") {
            layer.alert("请选择功能。");
            return;
        }
        $("#ParentId").val(selectedMenuId);
    }
    else {
        $("#ParentId").val("00000000-0000-0000-0000-000000000000");
    }
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#Code").val("");
    $("#Name").val("");
    $("#Type").val(0);
    $("#Url").val("");
    $("#Icon").val("");
    $("#SerialNumber").val(0);
    $("#Remarks").val("");
    $("#Title").text("新增顶级");
    //弹出新增窗体
    $("#addRootModal").modal("show");
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
        url: "/Menu/Get",
        data: { id: rows[0].id },
        success: function (data) {
            $("#Id").val(data.id);
            $("#ParentId").val(data.parentId);
            $("#Name").val(data.name);
            $("#Code").val(data.code);
            $("#Type").val(data.type);
            $("#Url").val(data.url);
            $("#Icon").val(data.icon);
            $("#SerialNumber").val(data.serialNumber);
            $("#Remarks").val("");

            $("#Title").text("编辑功能")
            $("#addRootModal").modal("show");
        }
    })
};
//保存
function save() {
    $.ajax({
        type: "Post",
        url: "/Menu/Edit",
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
//批量删除
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
            url: "/Menu/DeleteMuti",
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
//删除单条数据
function deleteSingle(id) {
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        $.ajax({
            type: "POST",
            url: "/Menu/Delete",
            data: { "id": id },
            success: function (data) {
                if (data.result == "Success") {
                    initTree();
                    layer.closeAll();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        })
    });
};

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
                    Val: selectedMenuId
                }
            ]
        };
        return temp;
    };
    oTableInit.Init = function () {
        $table.bootstrapTable({
            url: "/Menu/GetMneusByParent",
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
                field: 'serialNumber',
                title: '序号',
                align: "center"
            }, {
                field: "icon",
                title: "图标",
                align: "center",
                formatter: function (value, row, index) {
                    return "<i class=\"" + value + "\"></i>"
                }
            }, {
                field: "name",
                title: "名称"
            }, {
                field: "code",
                title: "编码"
            }, {
                field: "type",
                title: "类型",
                formatter: function (value, row, index) {
                    if (value == 0) {
                        return "功能菜单";
                    } else if (value == 1) {
                        return "操作按钮";
                    }
                }
            }, {
                field: "remarks",
                title: "备注"
            }]
        });
    };
    return oTableInit;
};