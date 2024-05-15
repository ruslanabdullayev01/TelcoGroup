document.addEventListener("DOMContentLoaded", (event) => {
    const newsButtons = document.querySelectorAll(".newsMiniButton");
    const newsLeft = document.querySelectorAll("#News .newsSlider .sliderLeft");
    const newsDesktop = document.querySelector("#News .newsSlider");
    document.querySelectorAll(".newsMiniButton").forEach((item) => {
        item.addEventListener("click", function () {
            let index = item.getAttribute("data-index");
            /*document.querySelector(".activeminibutton").classList.remove("activeminibutton");*/

            newsButtons.forEach((element) =>
                element.classList.remove("activeminibutton")
            );
            this.classList.add("activeminibutton");

            document
                .querySelector(".activesliderleft")
                .classList.remove("activesliderleft");
            document
                .querySelector(`.sliderLeft[data-target="${index}"]`)
                .classList.add("activesliderleft");

            // console.log("newsLeft=", newsLeft[index]);
            if (window.innerWidth < 700) {
                console.log("HEYYYY");
            }
            newsDesktop.style.backgroundImage = `url('src/images/svg/latestDesktop${Number(index) + 1
                }.svg')`;
            newsLeft[index].style.backgroundImage = `url('src/images/svg/latestnew${Number(index) + 1
                }.svg')`;
        });
    });

    //solutions
    const solutionButtons = document.querySelectorAll(".complexityMiniButton");

    document.querySelectorAll(".complexityMiniButton").forEach((item) => {
        item.addEventListener("click", function () {
            let index = item.getAttribute("data-index");
            /*document.querySelector(".activeminibutton").classList.remove("activeminibutton");*/

            solutionButtons.forEach((element) =>
                element.classList.remove("activeminibutton")
            );
            this.classList.add("activeminibutton");

            document
                .querySelector(".activeSolution")
                .classList.remove("activeSolution");
            document
                .querySelector(`.solutionSlider[data-target="${index}"]`)
                .classList.add("activeSolution");
        });
    });

    const splide = new Splide(".splide", {
        type: "loop",
        drag: "free",
        focus: "center",
        arrows: false,
        pagination: false,
        dots: false,
        perPage: 6,
        autoScroll: {
            speed: 1,
        },
    });

    splide.mount(window.splide.Extensions);
});