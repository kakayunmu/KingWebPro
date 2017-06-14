$(function () {
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $("#btnEdit").click(function () { edit(); });
    loadTables();
});
function add() {
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#Name").val("");
    $("#IDNumber").val("");
    $("#MobileNumber").val("");
    $("#Title").text("员工");
    //弹出新增窗体
    $("#addRootModal").modal("show");
}
function edit() {
    var rows = $table.bootstrapTable("getSelections");
    if (rows.length == 0) {
        layer.alert("请选中需要编辑的数据。");
        return;
    }
    if (rows.length > 1) {
        layer.msg("你选中的多行，默认取你选中的第一行进行编辑。");
    }
    var staff = rows[0];
    $("#Id").val(staff.id);
    $("#Name").val(staff.name);
    $("#MobileNumber").val(staff.mobileNumber);
    $("#IDNumber").val(staff.idNumber);

    $("#Title").text("编辑员工")
    $("#addRootModal").modal("show");
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
            url: "/Staff/DeleteMuti",
            data: sendData,
            success: function (data) {
                if (data.result == "Success") {
                    layer.closeAll();
                    loadTables();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        });
    });
}
function save() {
    $.ajax({
        type: "Post",
        url: "/Staff/Edit",
        data: $("#editForm").serialize(),
        success: function (data) {
            if (data.result == "Success") {
                loadTables();
                $("#addRootModal").modal("hide");
            } else {
                layer.tips(data.message, "#btnSave");
            };
        }
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
            url: "/Staff/GetStaff",
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
                field: 'name',
                title: '姓名'
            }, {
                field: "idNumber",
                title: "身份证"
            }, {
                field: "mobileNumber",
                title: "手机号"
            }, {
                field: "currentAmount",
                title: "活期账号金额"
            }, {
                field: "fixedAmount",
                title: "固定存款",
                align: "right"
            }, {
                field: "createTime",
                title: "创建时间",
                formatter: function (value) {
                    return moment(value).format("YYYY-MM-DD HH:mm:ss ");
                }
            }]
        });
    };
    return oTableInit;
};