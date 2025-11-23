export async function openModal(url) {

    function runScriptsFromElements(scriptElements, parent = document.body) {
        if (!scriptElements || scriptElements.length === 0) return;
        scriptElements.forEach(oldScript => {
            const newScript = document.createElement('script');
            if (oldScript.type === 'module') {
                import(oldScript.src)
                    .then(module => console.log('Module loaded', module))
                    .catch(err => console.error(err));
                return;
            }
            // Копируем все атрибуты
            for (let attr of oldScript.attributes) {
                newScript.setAttribute(attr.name, attr.value);
            }

            // Копируем содержимое (inline JS)
            newScript.textContent = oldScript.textContent;

            // Вставляем в DOM — скрипт выполнится
            parent.appendChild(newScript);
        });
    }

    let wrapper_element = document.createElement('div');
    wrapper_element.classList.add('modal-generated', 'wrapper');

    Object.assign(wrapper_element.style, {
        position: 'fixed',
        top: '0',
        left: '0',
        zIndex: '10000',
        backgroundColor: 'rgba(192, 192, 192, 0.5)',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh',
        width: '100vw'
    });

    let loading_animation = document.createElement('img');
    loading_animation.src = '/sources/loading.gif';
    wrapper_element.appendChild(loading_animation);
    wrapper_element.classList.add('modal-generated', 'load-animation');
    document.body.appendChild(wrapper_element);
    let virtual_dom;



    try {
        let response = await fetch(url);        
        if (!response.ok) throw new Error(`Modal generated error! Faild get element by http request \n Status: ${response.status} \n Message: ${response.body}`);
        let responseText = await response.text();
        console.log(responseText);
        let parser = new DOMParser();
        virtual_dom = parser.parseFromString(responseText, 'text/html');
        console.log(virtual_dom);
        const errorNode = virtual_dom.querySelector("parsererror");
        if (errorNode || !virtual_dom) {
            let error;
            if (!virtual_dom) error = "Virtual DOM is null or undefined";
            else error = errorNode.textContent.trim();

            throw new Error(`Modal generated error! Faild parse element from response \n Error: ${error}`);

        }
        
    }
    catch (error) {
        console.error(`Error: ${error.name} \n Error message: ${error.message}`);
        document.body.removeChild(wrapper_element);
        return;
    }
    let modal_element = virtual_dom.body.firstElementChild;


    if (!modal_element) {
        console.error("Modal element not found in response");
        document.body.removeChild(wrapper_element);
        return;
    }


    let script_elements = virtual_dom.querySelectorAll('script');
    console.log(script_elements[0]);
    let style_elements = virtual_dom.querySelectorAll('style');
    if (script_elements) runScriptsFromElements(script_elements);
    if (style_elements) style_elements?.forEach(style => document.head.appendChild(style));

    loading_animation.style.display = 'none';

    wrapper_element.appendChild(modal_element);
    wrapper_element.querySelectorAll('[modal-close="true"]').forEach(btn => {
        btn.onclick = () => closeModal();
    });

}

export function closeModal() {
    document.querySelectorAll('.modal-generated').forEach(element => element.remove());
}