$.ajax({
    //async: false,
    type: "get",
    url: "api/Pic/all",
    success: function (response) {
        console.log(response);
        response.forEach(element => {
            let pic = '<div align="center" style="margin-top:20px;flex:50%"><a><img style="width:80%" src="' + element.picFileName + '"></a>' +
            '<a><p><font color="gray" size=3>' + element.picName + '</font></p></a></div>';
            $(".picFrame").prepend(pic);
        });
        
    }
});