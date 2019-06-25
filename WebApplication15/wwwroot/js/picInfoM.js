//从url拿到图片的url
function getPic() {
    let url = window.location.href;
    let Params = url.split('?')[1];
    let PicPath = '/upload/' + Params.split('=')[1];
    return PicPath;
}

//用来显示上传了的图片
function init() {
    let PicPath = getPic();
    $("#picPreview").attr("src", PicPath);
}

init();

class Picture {
    constructor(picname, display, catalogue, fileName) {
        this.picname = picname;
        this.display = display;
        this.catalogue = catalogue;
        this.fileName = fileName;
    }
}

$("#uploadBtn").bind("click", () => {
    let AuthorizationString="Bearer "+$.cookie("token");
    let val=1;
    if($('.switch-anim').prop('checked')){
        //console.log("选中");
        val=1;
    }else{
        //console.log("没选中");
        val=2;
    }
    let pic = new Picture($("#picName").val(),val, $("#select1").val(), getPic());
    console.log(pic);
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "post",
        url: "api/pic/addPic",
        //将对象转为json格式
        data: JSON.stringify(pic),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            console.log(response);
            window.location.href="/mobile";
        },
        error: function(response){
            //console.log(response);
            
        }
    });
});
