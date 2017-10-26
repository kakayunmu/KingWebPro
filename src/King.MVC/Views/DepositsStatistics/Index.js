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
            url: "/DepositsStatistics/GetDepositsStatistics",
            method: "post",
            striped: true,
            pagination: true,
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            queryParams: oTableInit.queryParams,
            clickToSelect: true,
            columns: [{
                checkbox: true
            }, {
                field: 'name',
                title: '姓名'
            }, {
                field: "amount",
                title: "金额"
            }, {
                field: "remarks",
                title: "操作"
            },{
                field: "createTime",
                title: "时间",
                formatter: function (value) {
                    return $global.formatDate(value, 'yyyy-MM-dd hh:mm:ss')
                }
            }]
        });
    };
    return oTableInit;
};
function DoSearch() {
    loadTables();
}
