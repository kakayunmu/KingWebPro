$(function () {
    $("#saveBtn").on("click", saveSetting);
});
function saveSetting() {
    $.ajax({
        url: "/Setting/SaveSetting",
        type: "POST",
        data: $("#settingForm").serialize(),
        success: function (data) {
            if (data.status == 0) {
                $("#dataId").val(data.dataId);
                layer.msg("保存设置成功");
            } else {
                layer.alert(data.msg);
            }
        },
        error: function (ret) {
            layer.alert("保存设置失败");
        }
    });
};