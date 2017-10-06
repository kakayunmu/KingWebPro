$(function () {
    chart1();
    chart2();
    chart3();
    chart4();
});
function chart1() {
    var myChart = echarts.init(document.getElementById('chart1'));
    option = {
        title: {
            text: '活期定存比例',
            subtext: '当前所有获取和定存',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            left: 'left',
            data: ['定存', '活期']
        },
        series: [
            {
                name: '存款类型',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };

    myChart.setOption(option);
    //加载数据
    myChart.showLoading();
    $.post("/Home/GetChart1Data", {}, function (ret) {
        myChart.hideLoading();
        myChart.setOption({
            series: [{
                data: [
                    { value: $global.formatNumber(ret.data.totalFA, '#0.00'), name: '定存' },
                    { value: $global.formatNumber(ret.data.totalCA, '#0.00'), name: '活期' }                 
                ]
            }]
        });
    }, "JSON");
}
function chart2() {
    var myChart = echarts.init(document.getElementById('chart2'));
    option = {
        title: {
            text: '近两个月提现趋势'
        }, 
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross'
            },
            backgroundColor: 'rgba(245, 245, 245, 0.8)',
            borderWidth: 1,
            borderColor: '#ccc',
            padding: 10,
            textStyle: {
                color: '#000'
            },
            position: function (pos, params, el, elRect, size) {
                var obj = { top: 10 };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 30;
                return obj;
            },
            extraCssText: 'width: 170px'
        },
        legend: {
            data: ['提现金额', '提现人数']
        },
        dataZoom: [
            {
                show: true,
                realtime: true,
                start: 35,
                end: 100
            },
            {
                type: 'inside',
                realtime: true,
                start: 35,
                end: 100
            }
        ],
        xAxis: {
            type: 'category',
            data: []
        },
        yAxis: [{
            name: '提现金额',
            type: 'value',
            axisLabel: {
                formatter: '{value} 元'
            }
        }, {
            name: '提现人数',
            type: 'value',
            axisLabel: {
                formatter: '{value} 人'
            }
        }],
        series: [
            {
                yAxisIndex: 0,
                name: '提现金额',
                type: 'line',
                data: []
            },
            {
                yAxisIndex: 1,
                name: '提现人数',
                type: 'line',
                data: []
            }
        ]
    };
    myChart.setOption(option);
    myChart.showLoading();
    $.post("/Home/GetChart2Data", {}, function (ret) {
        myChart.hideLoading();
        myChart.setOption({
            xAxis: {
                data: ret.data.map(function (item) {
                    return item.applyTime;
                })
            },
            series: [{
                name: '提现金额',
                yAxisIndex: 0,
                data: ret.data.map(function (item) {
                    return $global.formatNumber(item.totalAmount, '#0.00');
                })
            }, {
                name: '提现人数',
                yAxisIndex: 1,
                data: ret.data2.map(function (item) {
                    return item.totalCount;
                })
            }]
        });
    }, "JSON");
}
function chart3() {
    var myChart = echarts.init(document.getElementById('chart3'));
    option = {
        title: {
            text: '未来可用固存',
            subtext: '未来可动用的资金'
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['固存']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            splitLine: { show: false },
            data: []
        },
        yAxis: {
            type: 'value'
        },
        series: [
            {
                name: '固存',
                type: 'bar',
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: []
            }
        ]
    };
    myChart.setOption(option);
    myChart.showLoading();
    $.post("/Home/GetChart3Data", {}, function (ret) {
        myChart.hideLoading();
        myChart.setOption({
            xAxis: {
                data: ret.data.map(function (item) {
                    return item.diffMonth;
                })
            },
            series: {
                data: ret.data.map(function (item) {
                    return $global.formatNumber(item.totalAmount, '#0.00');
                })
            }
        });
    }, "JSON");
}
function chart4() {
    var myChart = echarts.init(document.getElementById('chart4'));
    option = {
        title: {
            text: '近12个月成本趋势'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross'
            },
            backgroundColor: 'rgba(245, 245, 245, 0.8)',
            borderWidth: 1,
            borderColor: '#ccc',
            padding: 10,
            textStyle: {
                color: '#000'
            },
            position: function (pos, params, el, elRect, size) {
                var obj = { top: 10 };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 30;
                return obj;
            },
            extraCssText: 'width: 170px'
        },
        legend: {
            data: ['固存', '活期']
        },
        xAxis: {
            type: 'category',
            data: []
        },
        yAxis: [{
            name: '固存',
            type: 'value',
            axisLabel: {
                formatter: '{value} 元'
            }
        }, {
            name: '活期',
            type: 'value',
            axisLabel: {
                formatter: '{value} 元'
            }
        }],
        series: [
            {
                yAxisIndex: 0,
                name: '固存',
                type: 'line',
                data: []
            },
            {
                yAxisIndex: 1,
                name: '活期',
                type: 'line',
                data: []
            }
        ]
    };
    myChart.setOption(option);
    myChart.showLoading();
    $.post("/Home/GetChart4Data", {}, function (ret) {
        myChart.hideLoading();
        myChart.setOption({
            xAxis: {
                data: ret.data2.map(function (item) {
                    return item.createTimeMonth
                })
            },
            series: [{
                yAxisIndex: 0,
                data: ret.data.map(function (item) {
                    return $global.formatNumber(item.totalcumulativeAmount, '#0.00');
                })
            }, {
                yAxisIndex: 1,
                data: ret.data2.map(function (item) {
                    return $global.formatNumber(item.totalAmount, '#0.00');
                })
            }]
        });
    }, "JSON");
}