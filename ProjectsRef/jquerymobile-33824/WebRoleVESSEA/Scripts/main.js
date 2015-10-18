
function UserSurvey() {
    var queryString = $('#frmSurvey').serialize();
    makePOSTRequest("section_body", "PROJECT=BLL&NAMESPACE=BLL.Survey&CLASS=ResultItems&METHOD=PostSurveyResults&" + queryString);
    //alert(queryString);
}

function SiteLog(path, isAjax) {
    var parameters = "ACTION=SetClientSessionValues" + "&ClientWidth=" + document.documentElement.clientWidth + "&ClientHeight=" + document.documentElement.clientHeight + "&FileName=" + path + "&Ajax=" + isAjax;
    http_request = new XMLHttpRequest();
    http_request.open("post", "/SiteLog.aspx", true);
    http_request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http_request.setRequestHeader("Content-length", parameters.length);
    http_request.send(parameters);
};

function makePOSTRequest(section, parameters) {

    http_request = new XMLHttpRequest();
    http_request.open("POST", "/Requests.aspx", true);
    http_request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http_request.setRequestHeader("Content-length", parameters.length);
    http_request.send(parameters);    
    http_request.onreadystatechange = alertContents;

    function alertContents() {
        if (http_request.readyState == 4) {
            if (http_request.status == 200) {
                result = http_request.responseText;
                try {

                    top.parent.document.getElementById(section).innerHTML = result;

                    //Style the Ajax request
                    $("div[data-role=page]").page("destroy").page();

                    //Scroll to the top its a new page
                    jQuery('html,body').animate({ scrollTop: 0 }, 'slow');

                    SiteLog(parameters, true);

                }
                catch (e) {
                    alert("The HTTP Request could not find section: " + section + "! RE: Function - MakePostRequest")
                };

                try {
                    var node = top.parent.document.getElementById(section);
                }
                catch (e)
				{ };
                var st = node.getElementsByTagName("SCRIPT");
                for (var i = 0; i < st.length; i++) {
                    var strExec = st[i].text;
                    st[i].text = "";
                    var x = document.createElement("SCRIPT");
                    x.type = "text/javascript";
                    x.text = strExec;
                    try {
                        top.parent.document.getElementsByTagName("head")[0].appendChild(x);
                    }
                    catch (e) {
                        alert("Unable to append head!")
                    };
                }
            }
            else {
                alert(section);
                alert(parameters);
                alert("AJAX HTTP REQUEST ERROR IN DATA: " + http_request.status);
            }

        }

    }
};


function makePOST(parameters) {

    http_request = new XMLHttpRequest();
    http_request.open("POST", "/Requests.aspx", true);
    http_request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http_request.setRequestHeader("Content-length", parameters.length);
    http_request.send(parameters);
};


//// After more searching doing the followin removes the hash tag $.mobile.hashListeningEnabled = false;
//// http://stackoverflow.com/questions/7131909/facebook-callback-appends-to-return-url
//// Get rid of the Facebook residue hash in the URI postback
//// IE and Chrome version of the hack
//if (String(window.location.hash).substring(0, 1) == "#") {
//    window.location.hash = "";
//    window.location.href = window.location.href.slice(0, -1);
//}
//// Firefox version of the hack
//if (String(location.hash).substring(0, 1) == "#") {
//    location.hash = "";
//    location.href = location.href.substring(0, location.href.length - 3);
//}


$(document).bind("mobileinit", function () {

    var ThemeName = null;

    try {
        ThemeName = localStorage.getItem('DataKeyTheme');
    }
    catch (err) {
        ThemeName = "a"; //Set a default theme
        localStorage.setItem('DataKeyTheme', 'a');
    }

    if (ThemeName == null) {
        ThemeName = "a"; //Set a default theme
    }

    //http://stackoverflow.com/questions/7844006/facebook-authentication-and-strange-redirect-behaviour
    $.mobile.hashListeningEnabled = false;

    // Navigation
    $.mobile.page.prototype.options.backBtnText = "Go back";
    $.mobile.page.prototype.options.addBackBtn = true;
    $.mobile.page.prototype.options.backBtnTheme = ThemeName;

    // Page
    $.mobile.page.prototype.options.headerTheme = ThemeName;  // Page header only
    $.mobile.page.prototype.options.contentTheme = ThemeName;
    $.mobile.page.prototype.options.footerTheme = ThemeName;

    // Listviews
    $.mobile.listview.prototype.options.headerTheme = ThemeName;  // Header for nested lists
    $.mobile.listview.prototype.options.theme = ThemeName;  // List items / content
    $.mobile.listview.prototype.options.dividerTheme = ThemeName;  // List divider

    $.mobile.listview.prototype.options.splitTheme = ThemeName;
    $.mobile.listview.prototype.options.countTheme = ThemeName;
    $.mobile.listview.prototype.options.filterTheme = ThemeName;
    $.mobile.listview.prototype.options.filterPlaceholder = "Filter data...";

});

function ChangeTheme(theme) {

    localStorage.setItem('DataKeyTheme', theme);

    //reset all the buttons widgets
    $.mobile.activePage.find('.ui-btn')
                       .removeClass('ui-btn-up-a ui-btn-up-b ui-btn-up-c ui-btn-up-d ui-btn-up-e ui-btn-hover-a ui-btn-hover-b ui-btn-hover-c ui-btn-hover-d ui-btn-hover-e')
                       .addClass('ui-btn-up-' + theme)
                       .attr('data-theme', theme);

    //reset the header/footer widgets
    $.mobile.activePage.find('.ui-header, .ui-footer')
                       .removeClass('ui-bar-a ui-bar-b ui-bar-c ui-bar-d ui-bar-e')
                       .addClass('ui-bar-' + theme)
                       .attr('data-theme', theme);

    //reset the page widget
    $.mobile.activePage.removeClass('ui-body-a ui-body-b ui-body-c ui-body-d ui-body-e')
                       .addClass('ui-body-' + theme)
                       .attr('data-theme', theme);
};
