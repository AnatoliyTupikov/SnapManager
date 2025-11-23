import { getUrlSectionByIndex } from "./modules/get-url-section.mjs";
(function () {   
    window.addEventListener('load', function () {
        function saveConfigurationChapterState() {
            if (!window.sessionStorage) {
                console.log(`Browser doesn't suport "sessionStorage" \n page:"${window.location.pathname}"`);
                return;
            }
            let arr = [];
            let lb = document.querySelector("a.configuration-leave-button");
            if (!lb) console.log("no leave button found");
            else arr.push(lb);
            let mc = document.querySelector("a.header-config");
            if (!mc) console.log("no main config button found");
            else arr.push(mc);

            arr.forEach(el => {
                el.addEventListener('click', function (e) {   
                    let state = getUrlSectionByIndex(window.location.href, 2);
                    if (!state) return;
                    window.sessionStorage.setItem("snapshotManagerStorage.configuration_chapter_state", state );
                });
            });
        };
        saveConfigurationChapterState();

        function highlightLeftMenuItem() {

            let btns = document.querySelectorAll("a.left-menu-item");
            if (!btns)
            {
                console.log("no left items of the configuration menu found");
                return;
            }
            btns.forEach(el => {
                console.log(window.location.href.toLowerCase());
                console.log(el.href.toLowerCase())
                if (window.location.pathname.toLowerCase() === new URL(el.href).pathname.toLowerCase())
                {
                    el.setAttribute ("selected", "true");
                }
            })
        }
        highlightLeftMenuItem();
    });
    
})();