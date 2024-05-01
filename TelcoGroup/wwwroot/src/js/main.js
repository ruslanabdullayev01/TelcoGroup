//document.addEventListener('DOMContentLoaded', function () {
//    window.addEventListener('scroll', function () {
//        var navlinks = document.querySelectorAll('.navlink');
//        navlinks.forEach(function (navlink) {
//            navlink.classList.remove('activeNavLink');
//        });

//        var sections = ['#Video', '#AboutHome', '#Services', '#Complexity', '#Contact', '#News', '#Partners'];

//        sections.forEach(function (value, index) {
//            var section = document.querySelector(value);
//            if (window.pageYOffset+50 >= section.offsetTop) {
//                console.log(section.offsetTop)
//                navlinks.forEach(function (navlink) {
//                    navlink.classList.remove('activeNavLink');
//                });
//                navlinks[index].classList.add('activeNavLink');
//            }
//        });
//    });
//});

//document.addEventListener('DOMContentLoaded', function () {
//    window.addEventListener('scroll', function () {
//        var navlinks = document.querySelectorAll('.navlink');

//        var sections = ['#Video', '#AboutHome', '#Services', '#Complexity', '#Contact', '#News', '#Partners'];

//        sections.forEach(function (value, index) {
//            var section = document.querySelector(value);
//            if (isElementInViewport(section)) {
//                navlinks.forEach(function (navlink) {
//                    navlink.classList.remove('activeNavLink');
//                });
//                navlinks[index].classList.add('activeNavLink');
//            }
//        });
//    });

//    function isElementInViewport(element) {
//        var rect = element.getBoundingClientRect();
//        return (
//            rect.top <= (window.innerHeight / 2)
//        );
//    }
//});

//document.addEventListener('DOMContentLoaded', function () {
//    var navlinks = document.querySelectorAll('.navlink');

//    function scrollToSection(section) {
//        section.scrollIntoView({ behavior: 'smooth', block: 'center' });
//    }

//    navlinks.forEach(function (navlink) {
//        var sectionId = navlink.querySelector('a').getAttribute('href');
//        var section = document.querySelector(sectionId);

//        navlink.addEventListener('click', function (event) {
//            event.preventDefault();

//            scrollToSection(section);
//        });
//    });

//    window.addEventListener('scroll', function () {
//        var sections = ['#Video', '#AboutHome', '#Services', '#Complexity', '#Contact', '#News', '#Partners'];

//        sections.forEach(function (value, index) {
//            var section = document.querySelector(value);
//            if (isElementInViewport(section)) {
//                navlinks.forEach(function (navlink) {
//                    navlink.classList.remove('activeNavLink');
//                });
//                navlinks[index].classList.add('activeNavLink');
//            }
//        });
//    });

//    function isElementInViewport(element) {
//        var rect = element.getBoundingClientRect();
//        return (
//            rect.top <= (window.innerHeight / 2)
//        );
//    }
//});

document.addEventListener('DOMContentLoaded', function () {
    var navlinks = document.querySelectorAll('.navlink');

    navlinks.forEach(function (navlink) {
        var sectionId = navlink.querySelector('a').getAttribute('href');
        var section = document.querySelector(sectionId);

        navlink.addEventListener('click', function (event) {
            event.preventDefault(); 

            section.scrollIntoViewIfNeeded();
        });
    });

    window.addEventListener('scroll', function () {
        var sections = ['#Video', '#AboutHome', '#Services', '#Complexity', '#Contact', '#News', '#Partners'];

        sections.forEach(function (value, index) {
            var section = document.querySelector(value);
            if (isElementInViewport(section)) {
                navlinks.forEach(function (navlink) {
                    navlink.classList.remove('activeNavLink');
                });
                navlinks[index].classList.add('activeNavLink');
            }
        });
    });

    function isElementInViewport(element) {
        var rect = element.getBoundingClientRect();
        return (
            rect.top <= (window.innerHeight / 2)
        );
    }
});


//Mobile menu
const mobileMenuBtn = document.querySelector(".mobileMenuBtn");
const closeMenu = document.querySelector(".closeMenu");
const menuBar = document.querySelector(".menuBar");
const menuItems = document.querySelectorAll(".menuList .menuItem");

mobileMenuBtn.addEventListener("click", function () {
    menuBar.classList.add("showMobileMenu");
});

closeMenu.addEventListener("click", function () {
    menuBar.classList.remove("showMobileMenu");
});

menuItems.forEach(function (menuItem) {
    menuItem.addEventListener("click", function () {
        menuBar.classList.remove("showMobileMenu");
    });
});

//language div activator
const mobileLanguageDiv = document.querySelector(
    ".menuBar .menuOverlay .language"
);
const mobileAdditionalLanguageDiv = document.querySelector(
    ".menuBar .menuOverlay .language .langoptions"
);

mobileLanguageDiv.addEventListener("click", (e) => {
    e.stopPropagation();
    if (mobileAdditionalLanguageDiv.style.display == "block") {
        mobileAdditionalLanguageDiv.style.display = "none";
    } else {
        mobileAdditionalLanguageDiv.style.display = "block";
    }
});

//language changer
const mobileLanguages = document.querySelectorAll(
    ".menuBar .menuOverlay .language .lang"
);
const mobilePrevLang = document.querySelector(
    ".menuBar .menuOverlay .language .lang .selectedLang"
);

// const selectedLang
mobileLanguages.forEach((language) => {
    language.addEventListener("click", (e) => {
        const prevLangText = mobilePrevLang.textContent;
        const lang = language.textContent;
        e.target.textContent = prevLangText;
        mobilePrevLang.textContent = lang;
        mobileAdditionalLanguageDiv.style.display = "none";
    });
});