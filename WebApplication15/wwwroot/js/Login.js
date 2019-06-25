class LoginData {
    constructor(username, password) {
        this.username = username;
        this.password = password;
    }
}
document.getElementById('usernameLogIn').onfocus = () => {
    $("#LogErro").hide();
};
document.getElementById('PassLogIn').onfocus = () => {
    $("#LogErro").hide();
};

function updatePopup(){
    let AuthorizationString="Bearer "+$.cookie("token");
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "get",
        url: "api/Message",
        success: function (response) {
            if(response.length>0){
                $("#messageBox").html('<hr><button id="clear" type="button" class="btn btn-link btn-lg btn-block">清除所有</button>');
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

$("#logIn").bind("click", () => {
    let login = new LoginData($("#usernameLogIn").val(), $("#PassLogIn").val());
    console.log(login);
    if (login.username === "" || login.password === "") {
        $("#LogErro").children("strong").html("请输入完整的用户名密码");
        $("#LogErro").show();
        return;
    }
    $.ajax({
        type: "post",
        url: "api/User/authenticate",
        data: JSON.stringify(login),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            $(".close").click();
            console.log(response.userName);
            console.log(response);
            $.cookie('token', response.token);
           // $.cookie('id',response.id);
            $("#wtf").val(response.token);
            console.log($.cookie('token'));
            $("#addPicBtn").attr("data-target","#Upload");
           console.log($("addPicBtn"));
            let s='<h3>您好'+response.userName+',今天还可以上传一张照片</h3>';
            $("#userArea").replaceWith(s);
           // console.log($("#wtf").val());
           updatePopup();
        },
        error: function(response){
            console.log(response);
            $("#LogErro").children("strong").html(response.responseJSON.message);
            $("#LogErro").show();
        }
    });
});