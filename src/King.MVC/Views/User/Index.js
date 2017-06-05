var selectedDepartmentId = "00000000-0000-0000-0000-000000000000";
$(function () {
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
                field: "email",
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
                field: "reamrks",
                title: "备注"
            }]
        });
    };
    return oTableInit;
};
