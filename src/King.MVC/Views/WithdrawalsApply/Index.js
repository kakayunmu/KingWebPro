$(function () {
    loadTables();
    $("#btnAudit").on("click", fnAudit);
});

function fnAudit() {
    var rows = $table.bootstrapTable("getSelections");
    if (rows.length <= 0) {
        layer.msg("请选择需要处理的数据");
        return;
    }
    var flg = true;
    $.each(rows, function () {
        if (this.applyState != 0) {
            layer.msg("选择的数据中存在已审核过的数据，请重新选择");
            flg = false;
            return;
        }
    });
    if (!flg) {
        return;
    }
    layer.alert("请对你选择的数据选择操作", {
        btn: ['通过', '不通过', '取消'],
        yes: function (index, layero) {
            DoAudit(1);
            layer.closeAll('dialog');
        },
        btn2: function (index, layero) {
            DoAudit(2);
        },
        btn3: function (index, layero) {
            layer.closeAll('dialog');
        }
    });
};

function DoAudit(applyState) {
    var rows = $table.bootstrapTable("getSelections");
    var ids = new Array();
    $.each(rows, function () {
        ids.push(this.id);
    });
    $.post("/WithdrawalsApply/DoAudit",
        {
            ids: ids,
            applyState: applyState
        }, function (data) {
            if (data.status == 0) {
                loadTables();
                layer.msg("操作成功");
            } else {
                layer.alert(data.msg);
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
            url: "/WithdrawalsApply/GetWithdrawalsApplys",
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
                field: 'staffName',
                title: '姓名'
            }, {
                field: "staffMobileNumber",
                title: "手机号"
            }, {
                field: "amount",
                title: "提现金额"
            }, {
                field: "alipayAccount",
                title: "支付宝账号"
            }, {
                field: "applyTime",
                title: "提现时间",
                formatter: function (value) {
                    return moment(value).format("YYYY-MM-DD HH:mm:ss ");
                }
            }, {
                field: "auditor",
                title: "审核人"
            }, {
                field: "auditorTime",
                title: "创建时间",
                formatter: function (value) {
                    return moment(value).format("YYYY-MM-DD HH:mm:ss ");
                }
            }, {
                field: "applyState",
                title: "审核状态",
                formatter: function (value) {
                    switch (value) {
                        case 0:
                            return "未审核";
                        case 1:
                            return "审核通过";
                        case 2:
                            return "审核不通过";
                        case 3:
                            return "自动通过";
                    }
                }
            }]
        });
    };
    return oTableInit;
};