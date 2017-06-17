$(function () {
    $("#uploadBtn").on("click", function () { doUpload(); });
    $("#apply").on("click", function () { doCompare(); });
    $("#importbtn").on("click", function () { doWagesImport(); });
});

function doUpload() {
    var fileUpload = $("#wagesFile").get(0);
    var files = fileUpload.files;
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }
    $.ajax({
        url: "/WagesTemplate/UpLoadWagesFile?_t=" + new Date().getTime(),
        type: "POST",
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (ret) {
            if (ret.result == "Success") {
                layer.msg(ret.message);
                loadIndex = layer.load(2, { shade: [0.1, '#000'] });
                groupId = ret.data;
                importViewShow();
            } else {
                layer.alert("上传文件失败");
            }
        },
        error: function (ret) {
            layer.alert("上传文件失败");
        }
    });
};
var loadIndex;
var groupId;
function importViewShow() {

    $.post("/WagesTemplate/GetImportData", {
        groupId: groupId
    }, function (data) {
        if (data.result == "Success") {
            $("#importbox").hide();
            $("#importviewbox").show();
            var dotTmp = doT.template($("#importTemplate").html());
            $("#importViewArea").html(dotTmp(data.data));
        } else {
            layer.alert("获取导入的数据失败");
        }
        layer.close(loadIndex);
    }, "JSON");
}

function doCompare() {
    loadIndex = layer.load(2, { shade: [0.1, '#000'] });
    $.post("/WagesTemplate/DoCompare",
        {
            groupId: groupId
        }, function (ret) {
            if (ret.result == "Success") {
                $("#importviewbox").hide();
                $("#comparebox").show();
                var dotTmp = doT.template($("#compareTemlplate").html());
                $("#compareViewArea").html(dotTmp(ret.data));
                if (ret.flg) {
                    $("#importbtn").removeClass("disabled");
                } else {
                    $("#importTip").html("经系统检查，存在错误数据，请按照提示修改导入文件。然后重新执行 <a href='/wagesTemplate'>导入</a> 操作。");
                }
            } else {
                layer.alert("比较数据时发生错误");
            }
            layer.close(loadIndex);
        }, "JSON");
}
function doWagesImport() {
    loadIndex = layer.load(2, { shade: [0.1, '#000'] });
    $.post("/WagesTemplate/WagesImport",
        {
            groupId: groupId
        }, function (ret) {
            if (ret.result == "Success") {
                layer.msg("恭喜，导入工资成功");
                setTimeout(function () {
                    $("#comparebox").hide();
                    $("#importbox").show();
                }, 2000);
            } else {
                layer.alert("导入工资失败");
            }
            layer.close(loadIndex);
        }, "JSON");
}

