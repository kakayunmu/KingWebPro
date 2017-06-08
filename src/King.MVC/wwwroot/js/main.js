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