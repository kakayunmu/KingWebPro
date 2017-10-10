$(function () {
    loadTables();
    $("#seachBtn").on("click", DoSearch);
});
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
        var searchFelds = new Array();
        var staffName = $("#staffName").val();
        if (staffName != "") {
            searchFelds.push({ "Field": "staffName", "Val": staffName });
        }
        var stime = $("#stime").val();
        if (stime != "") {
            searchFelds.push({ "Field": "stime", "Val": stime });
        }
        var temp = {
            StartPage: params.offset / params.limit + 1,
            PageSize: params.limit,
            Order: params.order,
            Sort: params.sort,
            SearchFelds: searchFelds
        };
        return temp;
    };
    oTableInit.Init = function () {
        $table.bootstrapTable({
            url: "/WageStatistics/GetWageStatistics",
            method: "post",
            striped: true,
            pagination: true,
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100,200],
            queryParams: oTableInit.queryParams,
            clickToSelect: true,
            columns: [{
                checkbox: true
            }, {
                field: 'staffName',
                title: '姓名'
            }, {
                field: "mobileNumber",
                title: "手机号"
            }, {
                field: "idNumber",
                title: "身份证号"
            }, {
                field: "currDate",
                title: "月份",
                footerFormatter: function (rows) {
                    return "小计：";
                }
            }, {
                field: "wageAmount",
                title: "工资",
                formatter: function (value) {
                    return Math.round(value * 100) / 100;
                },
                footerFormatter: function (rows) {
                    var total = 0;
                    for (var i = 0; i < rows.length;i++){
                        total += rows[i].wageAmount;
                    }
                    return Math.round(total * 100) / 100;
                }
            }, {
                field: "currAmount",
                title: "活期利息",
                formatter: function (value) {
                    return Math.round(value * 100) / 100;
                },
                footerFormatter: function (rows) {
                    var total = 0;
                    for (var i = 0; i < rows.length; i++) {
                        total += rows[i].currAmount;
                    }
                    return Math.round(total * 100) / 100;
                }
            }, {
                field: "fixAmount",
                title: "定期利息",
                formatter: function (value) {
                    return Math.round(value * 100) / 100;
                },
                footerFormatter: function (rows) {
                    var total = 0;
                    for (var i = 0; i < rows.length; i++) {
                        total += rows[i].fixAmount;
                    }
                    return Math.round(total*100)/100;
                }
            }]
        });
    };
    return oTableInit;
};
function DoSearch() {
    loadTables();
}
