$(function () {
    //注册登出事件
    $("#btnSignOut").on("click", function (event) {
        event.preventDefault();
        fnSingOut();
    });
});

//用户登出方法
function fnSingOut() {
    $.post("/Login/SignOut",
        {},
        function (data) {
            if (data.result == "Success") {
                layer.msg("成功登出");
                window.location.href = "/Login";
            } else {
                layer.alert("退出登录失败");
            }
        }, "JSON");
}

(function (window) {
    var u = {};
    u.formatNumber = function (v, pattern) {
        if (v == null)
            return v;
        var symbol = v > 0 ? '' : '-';
        v = Math.abs(v);
        var strarr = v ? v.toString().split('.') : ['0'];
        var fmtarr = pattern ? pattern.split('.') : [''];
        var retstr = '';
        // 整数部分
        var str = strarr[0];
        var fmt = fmtarr[0];
        var i = str.length - 1;
        var comma = false;
        for (var f = fmt.length - 1; f >= 0; f--) {
            switch (fmt.substr(f, 1)) {
                case '#':
                    if (i >= 0) retstr = str.substr(i--, 1) + retstr;
                    break;
                case '0':
                    if (i >= 0) retstr = str.substr(i--, 1) + retstr;
                    else retstr = '0' + retstr;
                    break;
                case ',':
                    comma = true;
                    retstr = ',' + retstr;
                    break;
            }
        }
        if (i >= 0) {
            if (comma) {
                var l = str.length;
                for (; i >= 0; i--) {
                    retstr = str.substr(i, 1) + retstr;
                    if (i > 0 && ((l - i) % 3) == 0) retstr = ',' + retstr;
                }
            } else retstr = str.substr(0, i + 1) + retstr;
        }
        retstr = retstr + '.';
        // 处理小数部分
        str = strarr.length > 1 ? strarr[1] : '';
        fmt = fmtarr.length > 1 ? fmtarr[1] : '';
        i = 0;
        for (var f = 0; f < fmt.length; f++) {
            switch (fmt.substr(f, 1)) {
                case '#':
                    if (i < str.length) retstr += str.substr(i++, 1);
                    break;
                case '0':
                    if (i < str.length) retstr += str.substr(i++, 1);
                    else retstr += '0';
                    break;
            }
        }
        return symbol + retstr.replace(/^,+/, '').replace(/\.$/, '');
    };

    u.formatDate = function (v, format) {
        if (!v) return "";
        var d = v;
        if (typeof v === 'string') {
            if (v.indexOf("/Date(") > -1)
                d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
            else
                d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0])); //.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
        }
        var o = {
            "M+": d.getMonth() + 1, //month
            "d+": d.getDate(), //day
            "h+": d.getHours(), //hour
            "m+": d.getMinutes(), //minute
            "s+": d.getSeconds(), //second
            "q+": Math.floor((d.getMonth() + 3) / 3), //quarter
            "S": d.getMilliseconds() //millisecond
        };
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    };

    u.niceDate = function (v, format) {
        if (!v) return "";
        var d = v;
        if (typeof v === 'string') {
            if (v.indexOf("/Date(") > -1)
                d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
            else
                d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0])); //.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
        }
        var time = new Date().getTime();
        time = parseInt((time - d.getTime()) / 1000);
        var s;
        if (time > 0 && time < 60 * 10) { //十分钟内
            return '刚刚';
        } else if ((time < 60 * 60) && (time >= 60 * 10)) {
            //超过十分钟少于1小时
            s = Math.floor(time / 60);
            return s + '分钟前';
        } else if ((time < 60 * 60 * 24) && (time >= 60 * 60)) {
            //超过1小时少于24小时
            s = Math.floor(time / 60 / 60);
            return s + '小时前';
        } else if ((time < 60 * 60 * 24 * 3) && (time >= 60 * 60 * 24)) {
            //超过1天少于3天内
            s = Math.floor(time / 60 / 60 / 24);
            return s + '天前';
        } else {
            var o = {
                "M+": d.getMonth() + 1, //month
                "d+": d.getDate(), //day
                "h+": d.getHours(), //hour
                "m+": d.getMinutes(), //minute
                "s+": d.getSeconds(), //second
                "q+": Math.floor((d.getMonth() + 3) / 3), //quarter
                "S": d.getMilliseconds() //millisecond
            };
            if (/(y+)/.test(format)) {
                format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
            }
            for (var k in o) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
            return format;
        }
    };
    window.$global = u;
})(window);