    let flag1 = false;
    function OpenFilter()  
    {
        var button1 = document.getElementById("FilterOption1");
        var button2 = document.getElementById("FilterOption2");
        var button3 = document.getElementById("FilterOption3");
        var formFilter = document.getElementById("Filter");
       
        if (flag1 === false)
        {
            formFilter.classList.remove("CloseFilter");
            formFilter.classList.add("OpenFilter");

            button1.classList.remove("CloseOption");
            button1.classList.add("OpenOption");
            button2.classList.remove("CloseOption");
            button2.classList.add("OpenOption");
            button3.classList.remove("CloseOption");
            button3.classList.add("OpenOption");

            flag1 = true;
        }
        else
        {
            formFilter.classList.remove("OpenFilter");
            formFilter.classList.add("CloseFilter");

            button1.classList.remove("OpenOption");
            button1.classList.add("CloseOption");
            button2.classList.remove("OpenOption");
            button2.classList.add("CloseOption");
            button3.classList.remove("OpenOption");
            button3.classList.add("CloseOption");
            flag1 = false;
        }
    }