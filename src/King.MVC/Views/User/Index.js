var selectedDepartmentId = "00000000-0000-0000-0000-000000000000";
$(function () {
    $("#Roles").select2();
    $("#btnAdd").click(function () { add(); });
    $("#btnEdit").click(function () { edit(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $("#btnUpdatePwd").click(function () { updatePwd(); });
    $("#btnReset").click(function () { resetPwd(); });
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

};

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
                    Field: "departmentId",
                    Val: selectedDepartmentId
                }
            ]
        };
        return temp;
    };
    oTableInit.Init = function () {
        $table.bootstrapTable({
            url: "/User/GetUserByDepartent",
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
                field: 'userName',
                title: '用户名',
                align: "center"
            }, {
                field: "name",
                title: "姓名"
            }, {
                field: "eMail",
                title: "Email"
            }, {
                field: "mobileNumber",
                title: "负责人电话"
            }, {
                field: "createTime",
                title: "创建时间",
                formatter: function (value) {
                    return moment(value).format("YYYY-MM-DD HH:mm:ss ");
                }
            }, {
                field: "remarks",
                title: "备注"
            }]
        });
    };
    return oTableInit;
};

function add() {
    if (selectedDepartmentId == "00000000-0000-0000-0000-000000000000") {
        layer.alert("请选选择部门");
        return;
    }
    roleSelectInit();
    //弹出新增窗体
    $("#EditModal").modal("show");
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#UserName").val("");
    $("#Name").val("");
    $("#Email").val("");
    $("#MobileNumber").val("");
    $("#Remarks").val("");
};
function edit() {
    var rows = $table.bootstrapTable("getSelections");
    if (rows.length == 0) {
        layer.alert("请选中需要编辑的数据。");
        return;
    }
    if (rows.length > 1) {
        layer.msg("你选中的多行，默认取你选中的第一行进行编辑。");
    }
    roleSelectInit();
    $.ajax({
        type: "Post",
        url: "/User/Get",
        data: { id: rows[0].id },
        success: function (data) {
            $("#Id").val(data.id);
            $("#UserName").val(data.userName);
            $("#Name").val(data.name);
            $("#Email").val(data.eMail);
            $("#MobileNumber").val(data.mobileNumber);
            $("#Remarks").val(data.remarks);
            var selectRoles = [];
            $.each(data.userRoles, function () {
                selectRoles.push(this.roleId);
            });
            $("#Roles").select2().val(selectRoles).trigger("change");
            $("#Title").text("编辑用户")
            $("#EditModal").modal("show");
        }
    })
}

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
            url: "/User/DeleteMuti",
            data: sendData,
            success: function (data) {
                if (data.result == "Success") {
                    layer.msg("删除数据成功");
                    loadTables();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        });
    });
};

function updatePwd() {
    var rows = $table.bootstrapTable("getSelections");
    if (rows.length == 0) {
        layer.alert("请选中需要重置密码的用户。");
        return;
    }
    if (rows.length > 1) {
        layer.msg("你选中的多行，默认取你选中的第一个用户。");
    }
    $("#Password").val("");
    $("#ResetModal").modal("show");
};
function save() {
    var formData = $("#editForm").serializeArray();
    formData.push({ "name": "departmentId", "value": selectedDepartmentId });
    $.ajax({
        type: "Post",
        url: "/User/Edit",
        data: formData,
        success: function (data) {
            if (data.result == "Success") {
                $("#addRootModal").modal("hide");
                loadTables();
                layer.msg("保存数据成功");
                $("#EditModal").modal("hide");
            } else {
                layer.tips(data.message, "#btnSave");
            };
        }
    });
};

function resetPwd() {
    var rows = $table.bootstrapTable("getSelections");
    $.post("/User/ResetPwd",
        {
            userId: rows[0].id,
            password: $("#Password").val()
        }, function (data) {
            if (data.result == "Success") {
                layer.msg("重置密码成功");
            } else {
                layer.msg("重置密码失败，请稍后重试。")
            }
            $("#ResetModal").modal("hide");
        }, "JSON");
};

function roleSelectInit() {
    $.post("/User/GetAllRoles", {}, function (data) {
        $("#Roles option").remove();
        $.each(data, function () {
            $("#Roles").append(" <option value='" + this.id + "'>" + this.name + "</option>")
        });

    }, "JSON");
};
