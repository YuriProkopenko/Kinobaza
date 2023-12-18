/* Menu button open */

const menuBtn = document.querySelector('.menu__button');
const menuList = document.querySelector('.menu__list');

menuBtn.addEventListener('click', () => {
   menuBtn.classList.toggle('open');
   menuList.classList.toggle('open');
})

/* Menu open list close by press Esc */

window.addEventListener('keydown', (e) => {
   if (e.key == 'Escape') {
      menuBtn.classList.remove('open');
      menuList.classList.remove('open');
   }
})

/* Menu open list close by click outside of it */

menuBtn.addEventListener('click', event => {
   event._isClickWithInMenu = true;
});

menuList.addEventListener('click', event => {
   event._isClickWithInMenu = true;
});

document.body.addEventListener('click', event => {
   if (event._isClickWithInMenu) return;
   menuBtn.classList.remove('open');
   menuList.classList.remove('open');
})

/* Menu open list close if viewport is resizing */

window.addEventListener('resize', (e) => {
   if (window.innerWidth >= 847) {
      menuBtn.classList.remove('open');
      menuList.classList.remove('open');
   }
});

