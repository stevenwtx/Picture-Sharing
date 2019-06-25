function isMobile() {
    if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/Windows Phone/i)) 
        { window.location = "/account";} 
        else {  }

}

function initPopup(){
    if($.cookie("token")==undefined){
        $("#messageBox").html("你没有登录");
    }
    let AuthorizationString="Bearer "+$.cookie("token");
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "get",
        url: "api/Message",
        success: function (response) {
            if(response.length>0){
                $("#messageBox").html('<hr><button type="button" id="clear" class="btn btn-link btn-lg btn-block">清除所有</button>');
            }else{
                $("#messageBox").html('没有任何消息');
                return;
            }
            console.log(response);
            response.forEach(element => {
                let message= '<hr><li style="margin-top:20px; margin-right:30px">'+
                element.userName+'给你的'+element.picName+'点了赞</li>';
                $("#messageBox").prepend(message);
                $("#clear").bind("click", ()=> {
                    let AuthorizationString="Bearer "+$.cookie("token");
                    $.ajax({
                        headers:{
                            "Authorization":AuthorizationString
                        },
                        type: "put",
                        url: "api/Message/clear",
                        success: function (response) {
                            console.log(response);
                            $("#messageBox").html("没有任何消息");

                        }
                    });
                });
            });
        }
    });
}


$(document).ready(() => {
    isMobile();
    initPopup();
    //initMenu();
});