document.getElementById('year').innerText = new Date().getFullYear()


const generaCardProdotti = function (prodotti) {
    const row = document.getElementById("products-section")
    prodotti.forEach(prodotto => {
        const card = document.createElement("div")
        card.classList.add("card", "h-100", "mx-2", "mt-2", "d-flex", "flex-column", "shadow")
        card.addEventListener("click", function () {
            window.location.href = `details.html?productId=${prodotto._id}`;
        });
        const img = document.createElement("img")
        img.classList.add("card-img-top")
        img.setAttribute("src", prodotto.imageUrl)
        img.setAttribute("alt", "product image")
        const body = document.createElement("div")
        body.classList.add("card-body", "d-flex", "flex-column", "justify-content-around")
        const productName = document.createElement("h5")
        productName.classList.add("card-title", 'fst-italic')
        productName.textContent = prodotto.name

        const brand = document.createElement("p")
        brand.classList.add("card-text", "fw-bold")
        brand.textContent = prodotto.brand
        const price = document.createElement("p")
        price.classList.add("card-text", "lead")
        price.textContent = `€${prodotto.price}`
        body.appendChild(productName)

        body.appendChild(brand)
        body.appendChild(price)
        card.appendChild(img)
        card.appendChild(body)
        row.appendChild(card)
    })
}

const getAssets = function () {
    fetch('https://striveschool-api.herokuapp.com/api/product/', {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI2NjNkZTkzMTgxODQ0MjAwMTUzNzU5MTYiLCJpYXQiOjE3MTUzMzM0MjUsImV4cCI6MTcxNjU0MzAyNX0.RL2tSvTK9BzLZruwVZmBN0ZpWYeJbBANQWPwJKwcp38"
        }
    })
        .then((response) => {
            if (response.ok) {
                console.log(response)
                return response.json()
            } else {
                throw new Error('Errore nel recupero dei prodotti')
            }
        })
        .then((assetsArray) => {
            console.log('CI SONO!', assetsArray)
            generaCardProdotti(assetsArray)
        })
        .catch((err) => {
            console.log('ERRORE', err)
        })
}
getAssets()

