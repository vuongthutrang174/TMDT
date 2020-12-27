


    let flag = false;
    function OpenForm()  
    {
        var formAdd = document.getElementById("formAdd");
        var layer = document.getElementById("layer");

        if (flag === false)
        {
            formAdd.classList.remove("close");
            formAdd.classList.add("Openform");
            layer.classList.remove("close");
            layer.classList.add("Openlayer");
            flag = true;
        }
        else
        {
            formAdd.classList.remove("Openform");
            formAdd.classList.add("close");
            layer.classList.remove("Openlayer");
            layer.classList.add("close");
            flag = false;
        }
    }