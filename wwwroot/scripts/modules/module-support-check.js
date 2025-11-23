(function () {
    let script = document.createElement('script');
    if (!('noModule' in script)) {
        let page = window.location.pathname.toLowerCase();
        console.log(`Can't run module scripts on "${page}" view. Browser don't support it.`)
    }   
})();