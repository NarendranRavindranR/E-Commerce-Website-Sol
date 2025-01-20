/*!
    * Start Bootstrap - SB Admin v7.0.7 (https://startbootstrap.com/template/sb-admin)
    * Copyright 2013-2023 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */
    // 
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Initialize Swiper.js for the category carousel
    const categoryCarousel = document.querySelector('.category-carousel');
    if (categoryCarousel) {
        const categorySwiper = new Swiper('.category-carousel', {
            slidesPerView: 4, // Show 4 items at a time
            spaceBetween: 30, // Space between slides
            navigation: {
                nextEl: '.swiper-next',
                prevEl: '.swiper-prev',
            },
            loop: false, // Disable loop if you don't want continuous scrolling
        });
    }

});
