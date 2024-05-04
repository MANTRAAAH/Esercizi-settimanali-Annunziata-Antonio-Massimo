const searchInput = document.getElementById("input-group");
const searchIcon = document.querySelector(".button-search");

function toggleSearchBar() {
  if (
    searchInput.style.display === "none" ||
    searchInput.style.display === ""
  ) {
    // Se la barra di ricerca è nascosta, mostrala
    searchInput.style.display = "inline-block";
    searchInput.focus(); // Imposta il focus sull'input
  } else {
    // Altrimenti, nascondila
    searchInput.style.display = "none";
  }
}

// Aggiungi un event listener per gestire il click sull'icona di ricerca
searchIcon.addEventListener("click", toggleSearchBar);
