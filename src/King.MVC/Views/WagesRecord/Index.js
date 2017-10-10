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
            url: "/WagesRecord/GetWagesRecords",
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
                title: "金额"
            }, {
                field: "remark",
                title: "月份"
            }, {
                field: "createTime",
                title: "发放时间",
                formatter: function (value) {
                    return moment(value).format("YYYY-MM-DD HH:mm:ss ");
                }
            }]
        });
    };
    return oTableInit;
};

function DoSearch() {
    loadTables();
}