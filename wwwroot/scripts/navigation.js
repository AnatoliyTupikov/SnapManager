import { openModal } from "./modules/modal.mjs";
window.addEventListener("load", function () {
    if (!document.getElementById("get-CredManager")) return;
    document.getElementById("get-CredManager").onclick = function () {
        openModal('https://localhost:5005/home/CredentialsWizard');
    };
});