let AuthorizationString="Bearer "+$.cookie("token");
$("#Test").bind("click", ()=> {
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