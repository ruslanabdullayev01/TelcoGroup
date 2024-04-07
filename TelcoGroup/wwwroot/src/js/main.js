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
