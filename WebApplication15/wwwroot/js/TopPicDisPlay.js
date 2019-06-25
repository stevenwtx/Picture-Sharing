$.ajax({
    //async: false,
    type: "get",
    url: "api/Pic/top",
    success: function (response) {
        console.log(response);
        for(let i=0;i<response.length;i++){
            if(i===0){
                let p='<div class="carousel-item active"><img src="'+response[0].path+'">'+
                '<div class="carousel-caption"><h3 style="color:black">'+response[0].name+'</h3>'+'</div></div>';
                $("#topPic").append(p);
            }else{
                let p='<div class="carousel-item"><img src="'+response[i].path+'">'+
                '<div class="carousel-caption"><h3 style="color:black">'+response[i].name+'</h3>'+'</div></div>';
                $("#topPic").append(p);
            }
        }
       
    },
    error: function (response) {
        console.log(response);
    }
});