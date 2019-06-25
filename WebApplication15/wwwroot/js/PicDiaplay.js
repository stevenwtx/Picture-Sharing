let pictures = undefined;

class like {
    constructor(pictureId) {
        this.pictureId = pictureId;
        this.tokenString = $.cookie("token");
    }
}
function display() {
    let pictures = undefined;
    let Likeid=undefined;
    $.ajax({
       async: false,
        type: "get",
        url: "api/Pic/all",
        success: function (response) {
            console.log(response);
            pictures = response;
            if($.cookie("token")!=undefined){
                console.log("aaaa");
                let AuthorizationString="Bearer "+$.cookie("token");
                $.ajax({
                    headers:{
                        "Authorization":AuthorizationString
                    },
                    async: false,
                    type: "get",
                    url: "api/Pic/like",
                    success: function (response) {
                        console.log(response);
                        Likeid=response;
                        console.log(Likeid);
                    }
                });
            }

        }
    });
    pictures.forEach(element => {
        if(element.id===Likeid){
            console.log(element.id);
            let pic = '<div class="col-md-3 col-sm-4 img_div" align="center" style="margin-top:20px"><a><img class="itemimg" style="width:100%" src="' + element.picFileName + '"></a>' +
            '<div class="mask"><a pid="' + element.id + '" class="glyphsSpriteHeart__filled__24__red_5 u-__7"></a></div>' +
            '<a><p><font color="gray" size=5>' + element.picName + '</font></p></a></div>';
            $("#todayFrame").append(pic);
        }else{
            console.log(element.id);
            console.log(Likeid);
            let pic = '<div class="col-md-3 col-sm-4 img_div" align="center" style="margin-top:20px"><a><img class="itemimg" style="width:100%" src="' + element.picFileName + '"></a>' +
            '<div class="mask"><a pid="' + element.id + '" class="glyphsSpriteHeart__outline__24__grey_9 u-__7 likeThisBtn"></a></div>' +
            '<a><p><font color="gray" size=5>' + element.picName + '</font></p></a></div>';
            $("#todayFrame").append(pic);
        }
       
        // $(".likeThisBtn").bind("click", function () {
        //     console.log($(this).attr("pid"));
        // });
        
        console.log($("#todayFrame"));
    });

    $(".likeThisBtn").bind("click", function () {
        //console.log('aaa');
        let pictureId = $(this);
        console.log(pictureId);
        let l = new like(pictureId.attr("pid"));
        let AuthorizationString = "Bearer " + l.tokenString;
        //let AuthorizationString = "Bearer " + $("#wtf").val();
        console.log(AuthorizationString + "from like");
        //console.log(l.userId);
        //console.log(l.pictureId);
        $.ajax({
            headers: {
                "Authorization": AuthorizationString
            },
            type: "post",
            url: "api/Pic/likeThis",
            data: JSON.stringify(l),
            //data:data,
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                console.log(response);
                $(pictureId).attr("class", "glyphsSpriteHeart__filled__24__red_5 u-__7");
            },
            error: function(response){
                console.log(response);
                if(response.status===401){
                    alert("请先登录");
                }else{
                    alert(response.responseText);
                }
               
            }
        });
    });

    

}

display()




