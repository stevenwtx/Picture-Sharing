function isMobile() {
    if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/Windows Phone/i))
        return true;
    return false;
}

function initUser(){
    let AuthorizationString = "Bearer " + $.cookie("token");
    if ($.cookie('token') != undefined) {
        console.log(AuthorizationString);
        $.ajax({
            headers: {
                "Authorization": AuthorizationString
            },
            async: true,
            type: "get",
            url: "api/User/" + $.cookie('token'),
            success: function (response) {
                console.log(response);
                let s = '<h3>您好' + response.userName + ',今天还可以上传一张照片</h3>';
                if (response.uploadedable === 'no') {
                    s = '<h3>您好' + response.userName + ',今天您已经上传过照片了</h3>';
                }

                $("#userArea").replaceWith(s);
                //$("#homeBtn").attr("data-target","");
               
            }
        });
    }else{
        $("#addPicBtn").attr("data-target","#login");
        $("#likeBtn").attr("data-target","#login");
        
    }
    $("#homeBtn").bind("click",()=>{
        if ($.cookie('token') != undefined){
            window.location.href="/user";
        }else{
            $("#loginbtn").click();
        }
        
    });

}

function initNum(){
    $.ajax({
        type: "get",
        url: "api/Pic/NumLeft",
        success: function (response) {
            console.log(response);
            $(".SpaceLeft").html(response);
        }
    });
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

function initMenu(){
    $.ajax({
        async:false,
        type: "get",
        url: "api/Pic/cata",
        //data:data,
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            console.log(response);
    
            response.forEach((e,index) => {
                let li = '<li class="nav-item"><a catid="'+index+'" class="nav-link">' + e + '</a></li>';
                $("#picNav").append(li);
                console.log(li);
            });
            $(".nav-link").bind("click", function(){
                let cid=$(this).attr("catid");
                $("#todayFrame").html("");
                console.log(cid);
                $.ajax({
                    type: "get",
                    url: "api/Pic/byCata",
                    data: {"cid":cid},
                    //dataType: "dataType",
                    success: function (response) {
                        console.log(response);
                        response.forEach(element=>{
                            let pic = '<div class="col-md-3 col-sm-4 img_div" align="center" style="margin-top:20px"><a><img class="itemimg" style="width:100%" src="' + element.picFileName + '"></a>' +
                            '<div class="mask"><a pid="' + element.id + '" class="glyphsSpriteHeart__outline__24__grey_9 u-__7"></a></div>' +
                            '<a><p><font color="gray" size=5>' + element.picName + '</font></p></a></div>';
                            $("#todayFrame").append(pic);
                        })
                    }
                });
            });
        }
    });
    
}

$(document).ready(() => {
    if (
        isMobile()
    ) {

        window.location = "/mobile";
    };
    initUser();
    initMenu();
    initNum();
    initPopup();
});