$(document).ready(function () {
    var mn = $(".navbar.navbar-default");
    var mns = "navbar-fixed-top";
    var hdr = $('#header').height();

    $(window).scroll(function () {
        if ($(this).scrollTop() > (hdr + 80)) {
            mn.addClass(mns);
        } else {
            mn.removeClass(mns);
        }
    });
})