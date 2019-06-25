class Picture {
    constructor(picname, display, catalogue, fileName) {
        this.picname = picname;
        this.display = display;
        this.catalogue = catalogue;
        this.fileName = fileName;
    }
}

$("#input_file").on("change", () => {
    let file = document.getElementById("input_file").files[0];
    //let files=file.files;
    //console.log(files);
    let picInForm = new FormData();
    let AuthorizationString = "Bearer " + $.cookie("token");
    console.log(file.name);
    picInForm.append(file.name, file);
    //console.log(fileObj1);
    console.log(picInForm);
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "post",
        url: "api/Pic/upLoadPic",
        data: picInForm,
        contentType: false,
        //contentType:"multipart/form-data; boundary=------",
        processData: false,
        success: function (response) {
            //console.log("/image/" + response.fileName);
            $("#picPreview").attr("src", "/upload/" + response.fileName);
            //console.log($("#picPreview"));
            $("#uploadBtn").bind("click", () => {
                let pic = new Picture($("#picName").val(), $('input[name="display"]:checked').val(), $("#select1").val(), "/upload/" + response.fileName);
                console.log(pic);
                $.ajax({
                    headers:{
                        "Authorization":AuthorizationString
                    },
                    type: "post",
                    url: "api/pic/addPic",
                    data: JSON.stringify(pic),
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        console.log(response);
                        // file.value="";
                        // $("#picPreview").attr("src","");
                        // $("#fileForm")[0].reset();
                        // $(".close").click();
                       
                        // let s = '<h3>您好,今天您已经上传过照片了</h3>';
                        
                        // $("#userArea").replaceWith(s);
                        // console.log("aaaa");
                        window.location.reload();
                    },
                    error: function(response){
                        //console.log(response);
                        
                    }
                });
            });
        }
    });

})

$(".filebtn").bind("click",()=>{
    $("#fileInput").click();
})