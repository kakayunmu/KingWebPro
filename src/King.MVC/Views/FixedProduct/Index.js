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
    $("#TimeLimit").val("");
    $("#AIRate").val("");
    $("#TimeLimitUnit").val(0);
    $("#DataState").val(0);
    $("#Title").text("新增产品");
    //弹出新增窗体
    $("#addRootModal").modal("show");
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
    var fixedProduct = rows[0];
    $("#Id").val(fixedProduct.id);
    $("#Name").val(fixedProduct.name);
    $("#TimeLimit").val(fixedProduct.timeLimit);
    $("#AIRate").val(fixedProduct.aiRate);
    $("#TimeLimitUnit").val(fixedProduct.timeLimitUnit);
    $("#DataState").val(fixedProduct.dataState);
    $("#IsHot").val(fixedProduct.isHot);

    $("#Title").text("编辑员工")
    $("#addRootModal").modal("show");
};
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
            url: "/FixedProduct/DeleteMuti",
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
};
function save() {
    $.ajax({
        type: "Post",
        url: "/FixedProduct/Edit",
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
            url: "/FixedProduct/GetFP",
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
                title: '产品名称'
            }, {
                field: "timeLimit",
                title: "期限",
                formatter: function (value, row, index) {
                    if (row.timeLimitUnit == 0) {
                        return value + "天";
                    } else {
                        return value + "个月";
                    }
                }
            }, {
                field: "aiRate",
                title: "年化利率",
                formatter: function (value) {
                    return value + "%";
                }
            }, {
                field: "isHot",
                title: "推荐级别"                
            }, {
                field: "dataState",
                title: "状态",
                formatter: function (value) {
                    if (value == 0) {
                        return "正常";
                    } else {
                        return "已下架";
                    }
                }
            }]
        });
    };
    return oTableInit;
};