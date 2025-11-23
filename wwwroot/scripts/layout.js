import { getUrlSectionByIndex } from "./modules/get-url-section.mjs";
(function () {   
    window.addEventListener('load', function () {

        function getLinkForConfButon() {
            let conf_el = document.querySelector('a.header-config');
            if (!conf_el) return;
            if (getUrlSectionByIndex(window.location.href, 1)?.toLowerCase() === 'configuration')
            {
                conf_el.href = '/';
                conf_el.setAttribute('selected', 'true'); 
            }
            else {
                if (!window.sessionStorage) console.log(`Browser doesn't suport "sessionStorage" \n page:"${window.location.pathname}"`);
                let adding = window.sessionStorage.getItem('snapshotManagerStorage.configuration_chapter_state') ?? "dbsettings";
                conf_el.href = '/configuration' + '/' + adding;
            }
        }
        getLinkForConfButon();

        function headerCanvasDraw() {
            if (!Modernizr.canvas)
            {
                console.log("The browser doesn't support the canvas");
                return;
            }
            let headerCanvas = document.querySelector('.header-canvas');
            if (!headerCanvas || !headerCanvas.getContext)
            {
                console.log("No canvas element found in the header");
                return;
            }
            let context = headerCanvas.getContext('2d');
            let grad = context.createLinearGradient(0, headerCanvas.height / 2.5, headerCanvas.width, headerCanvas.height / 2);
            grad.addColorStop(0.3, '#FFFFFF');
            grad.addColorStop(0.35, '#FF9933');
            grad.addColorStop(0.7, '#606060');
            context.fillStyle = grad;
            context.rect(0, 0, headerCanvas.width, headerCanvas.height);
            context.fill();
            
        }
        headerCanvasDraw();
    });
})();