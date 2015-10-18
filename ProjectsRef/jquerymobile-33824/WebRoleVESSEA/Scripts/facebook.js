//view-source:http://thinkdiff.net/demo/newfbconnect1/newconnect.php
//view-source:http://thinkdiff.net/demo/newfbconnect1/jssdkouth2.html

var FacebookAppID;

$.ajax({
    type: "POST",
    url: "/WebMethods.aspx/GetFacebookAppID",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (data) {
        if (data['d'] != "") {
            FacebookAppID = data['d'];
        } else {
            alert("Could not get FacebookSecret");
        }
    }
});

window.fbAsyncInit = function () {
    FB.init({
        appId: FacebookAppID,
        status: true,
        cookie: true,
        xfbml: true
    });
};
(function () {
    var e = document.createElement('script');
    e.type = 'text/javascript';
    e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
    e.async = true;
    document.getElementById('fb-root').appendChild(e);
}());

