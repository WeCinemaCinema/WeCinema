let slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
  showSlides((slideIndex += n));
}

function currentSlide(n) {
  showSlides((slideIndex = n));
}

function showSlides(n) {
  let i;
  let slides = document.getElementsByClassName("mySlides");
  if (n > slides.length) {
    slideIndex = 1;
  }
  if (n < 1) {
    slideIndex = slides.length;
  }
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  slides[slideIndex - 1].style.display = "block";
}

function activateButton(button, tabId) {
  const buttons = document.querySelectorAll(".title-2 button");
  buttons.forEach((btn) => btn.classList.remove("active"));

  button.classList.add("active");

  const tabContents = document.querySelectorAll(".tabcontent");
  tabContents.forEach((tab) => (tab.style.display = "none"));

  document.getElementById(tabId).style.display = "flex";
}

document.querySelector(".title-2 button.active").click();
