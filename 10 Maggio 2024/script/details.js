document.getElementById('year').innerText = new Date().getFullYear();

const addressBarContent = new URLSearchParams(location.search);
const productID = addressBarContent.get('productId');

const getProductDetails = async function () {
    try {
        if (!productID) {
            throw new Error("ID prodotto non valido");
        }

        const response = await fetch(`https://striveschool-api.herokuapp.com/api/product/${productID}`, {
            headers: {
                'Content-Type': 'application/json',
                "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI2NjNkZTkzMTgxODQ0MjAwMTUzNzU5MTYiLCJpYXQiOjE3MTUzMzM0MjUsImV4cCI6MTcxNjU0MzAyNX0.RL2tSvTK9BzLZruwVZmBN0ZpWYeJbBANQWPwJKwcp38"
            }
        });
        if (!response.ok) {
            throw new Error('Errore nel recupero delle informazioni richieste');
        }

        const prodotto = await response.json();
        console.log(prodotto);
        document.getElementById("image").setAttribute("src", prodotto.imageUrl);
        document.getElementById("name").innerText = prodotto.name;
        document.getElementById("description").innerText = prodotto.description;
        document.getElementById("brand").innerText = prodotto.brand;
        document.getElementById("price").innerText = "€" + prodotto.price;
    } catch (error) {
        console.log('ERRORE', error);
    }
};



getProductDetails();

const bottoneModifica = document.getElementById('edit-button')
bottoneModifica.addEventListener('click', function () {
    location.assign(`backoffice.html?productId=${productID}`)
})
