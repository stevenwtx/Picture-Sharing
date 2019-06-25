function isMobile() {
    if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/Windows Phone/i)) 
        { } 
        else { window.location = "/home"; }

}
isMobile();