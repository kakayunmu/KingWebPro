$(function () {
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $("#btnEdit").click(function () { edit(); });
    $("#btnSaveMenu").click(function () { savemenu(); });
    loadTables();
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
                    "multiple": true    //是否多选
                },
                "plugins": ["state", "types", "wholerow", "checkbox"],  //配置信息
                "checkbox": {
                    "keep_selected_style": false
                }
            })
            $("#treeDiv").on("ready.jstree", function (e, data) {   //树创建完成事件
                data.instance.open_all();    //展开所有节点
            });
            //$("#treeDiv").on('changed.jstree', function (e, data) {   //选中节点改变事件
            //    var node = data.instance.get_node(data.selected[0]);  //获取选中的节点
            //    if (node) {
            //        //selectedMenuId = node.id;
            //        //loadTables();
            //    };
            //});
        }
    });

}

//新增
function add() {
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#Code").val("");
    $("#Name").val("");
    $("#Remarks").val("");
    $("#Title").text("新增角色");
    //弹出新增窗体
    $("#EditModal").modal("show");
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
        url: "/Role/Get",
        data: { id: rows[0].id },
        success: function (data) {
            $("#Id").val(data.id);
            $("#Name").val(data.name);
            $("#Code").val(data.code);
            $("#Remarks").val(data.remarks);
            $("#Title").text("编辑功能")
            $("#EditModal").modal("show");
        }
    })
};
//保存
function save() {
    $.ajax({
        type: "Post",
        url: "/Role/Edit",
        data: $("#editForm").serialize(),
        success: function (data) {
            if (data.result == "Success") {
                $("#EditModal").modal("hide");
                layer.msg("数据保存成功");
                loadTables();
                $("#EditModal").modal("hide");
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
            url: "/Role/DeleteMuti",
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

//保存功能
function savemenu() {
    var selectedMenus = $('#treeDiv').jstree().get_selected();
    var selectedRoleId = "";
    var selectedRoles = $table.bootstrapTable("getSelections");
    if (selectedRoles.length == 0) {
        layer.alert("请选则需要设置的角色！");
        return;
    }
    if (selectedRoles.length > 1) {
        layer.msg("选中多个角色，无法保存，请选中一个角色设置。");
        return;
    }
    selectedRoleId = selectedRoles[0].id;
    var permissions = [];
    $.each(selectedMenus, function () {
        permissions.push({ "RoleId": selectedRoleId, "MenuId": this });
    });

    //询问框
    layer.confirm("确定要保存当前设置的功能吗？", {
        btn: ["确定", "取消"]
    }, function () {
        var sendData = { "roleId": selectedRoleId, "permissions": permissions };
        $.ajax({
            type: "Post",
            url: "/Role/SaveMenu",
            data: sendData,
            success: function (data) {
                if (data.result == "Success") {
                    layer.msg("保存数据成功");
                    loadTables();
                }
                else {
                    layer.alert("保存失败！");
                }
            }
        });
    });
}

var TableInit = function () {
    var oTableInit = new Object();
    oTableInit.queryParams = function (params) {
        var temp = {
            StartPage: params.offset / params.limit + 1,
            PageSize: params.limit,
            Order: params.order,
            Sort: params.sort
        };
        return temp;
    };
    oTableInit.Init = function () {
        $table.bootstrapTable({
            url: "/Role/GetList",
            method: "post",
            striped: true,
            pagination: true,
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            queryParams: oTableInit.queryParams,
            clickToSelect: true,
            onCheck: function (row, $element) {
                var rows = $table.bootstrapTable("getSelections");
                if (rows.length == 1) {
                    $.ajax({
                        type: "POST",
                        url: "/Role/GetMenusByRole?roleId=" + rows[0].id + "&_t=" + new Date().getTime(),
                        success: function (data) {
                            $('#treeDiv').jstree().deselect_all();
                            $('#treeDiv').jstree().select_node(data);
                        }
                    });
                } else {
                    layer.msg("当前未选中或选中多个角色，功能树无法显示");
                    $('#treeDiv').jstree().deselect_all();
                }
            },
            columns: [{
                checkbox: true
            }, {
                field: "name",
                title: "名称"
            }, {
                field: "code",
                title: "编码"
            }, {
                field: "remarks",
                title: "备注"
            }]
        });
    };
    return oTableInit;
};
