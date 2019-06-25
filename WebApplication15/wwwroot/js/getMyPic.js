$("._9VEo1").bind("click", function (e) {
    let val=$(this).attr("val");
    if(val==1){
        $("#private").attr("class","_9VEo1");
        $("#public").attr("class","_9VEo1 T-jvg");
    }else{
        $("#private").attr("class","_9VEo1 T-jvg");
        $("#public").attr("class","_9VEo1");
    }
    let AuthorizationString="Bearer "+$.cookie("token");
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "get",
        url: "api/Pic/byUser",
        data: {"visible":val},
        
        success: function (response) {
            console.log(response);
            $("#picFrame").html("");
            response.forEach(element => {
                let p=
                    '<div class="col-md-4 col-sm-4" align="center" style="margin-top:15px">'+
                    ' <a><img class="" style="width:100%" src="'+element.path+'"></a>'+
                    '<p style="margin-top:5px"><font color="gray" size=5>'+element.name+'</font></p></div>';     
                $("#picFrame").append(p);                               
            });
        }
    });
});

$("#public").click();