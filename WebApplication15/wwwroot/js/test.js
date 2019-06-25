class like{
    constructor(pictureId,userId){
        this.pictureId=pictureId;
        this.userId=userId;
    }
}
$("#todayFrame .likebtn").bind("click", function() {
    
    let AuthorizationString="Bearer "+$.cookie("token");
    //let AuthorizationString = "Bearer " + $("#wtf").val();
    console.log(AuthorizationString+"from wtf");
    let userId=1;
    let pictureId=6;
    let l=new like(pictureId,userId); 
    console.log(l.userId);
    console.log(l.pictureId);
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "post",
        url: "api/Pic/likeThis",
        data: JSON.stringify(l),
        //data:data,
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            console.log(response);
        }
    });
});
