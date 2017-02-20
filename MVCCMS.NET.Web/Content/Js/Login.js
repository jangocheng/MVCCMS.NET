//=============================屏幕宽度自适应======================================
$(function () {
    $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
    $(window).resize(function () {
        $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
    });
});
//=============================切换验证码======================================
function ClickRemoveChangeCode() {
    var code = $("#imgCode").attr("src");
    $("#imgCode").attr("src", code + "1");
}

//=============================提示框弹出======================================
function AlertJsonModel(e) {
    if (e.statusCode == 200) {
        window.location.href = e.forward;
    } else {
        layer.msg(e.message, { shift: 6 });
    }
}

//=============================关于我们======================================
function About() {
    layer.tips("怀化立天世纪科技有限公司<br>QQ:27888677", '#aboutus', {
        tips: 2
    });
}