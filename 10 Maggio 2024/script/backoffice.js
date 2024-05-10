document.getElementById('year').innerText = new Date().getFullYear();



class Prodotto {
    constructor(_name, _description, _brand, _price, _imageUrl) {
        this.name = _name;
        this.description = _description;
        this.brand = _brand;
        this.price = _price;
        this.imageUrl = _imageUrl;

    }
};

const addressBarContent = new URLSearchParams(location.search)
const productID = addressBarContent.get('productId')

let createProductForm;
const submitProduct = function (e) {
    e.preventDefault();
    const nameInput = document.getElementById('name').value
    const descriptionInput = document.getElementById('description').value
    const brandInput = document.getElementById('brand').value
    const priceInput = document.getElementById('price').value
    const imgInput = document.getElementById('imageUrl').value

    const createProductForm = new Prodotto(
        nameInput,
        descriptionInput,
        brandInput,
        priceInput,
        imgInput,
    );
    let URL = 'https://striveschool-api.herokuapp.com/api/product/'
    let metodo = 'POST'

    if (productID) {
        URL = `https://striveschool-api.herokuapp.com/api/product/${productID}`
        metodo = 'PUT'
    }

    fetch(URL, {
        method: metodo,
        body: JSON.stringify(createProductForm),
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI2NjNkZTkzMTgxODQ0MjAwMTUzNzU5MTYiLCJpYXQiOjE3MTUzMzM0MjUsImV4cCI6MTcxNjU0MzAyNX0.RL2tSvTK9BzLZruwVZmBN0ZpWYeJbBANQWPwJKwcp38"
        }
    })
        .then((response) => {
            if (response.ok) {
                alert('Prodotto Aggiunto')
            } else {
                throw new Error('Errore nel salvataggio del prodotto')
            }
        })
        .catch((err) => {
            console.log('ERRORE', err)
            alert("ATTENZIONE" + err)
        })
}
document.getElementById('product-form').addEventListener('submit', submitProduct)

const resetForm = function () {
    if (confirm("Sei sicuro di voler resettare il form?")) {
        document.getElementById('name').value = '';
        document.getElementById('description').value = '';
        document.getElementById('brand').value = '';
        document.getElementById('price').value = '';
        document.getElementById('imageUrl').value = '';
    }
};

const resetButton = document.getElementById('reset-button')
resetButton.addEventListener('click', resetForm);

const btnCrea = document.getElementById('btn-crea');
if (productID) {
    btnCrea.textContent = "Modifica Prodotto";
}

let modifyProduct

const productData = function () {
    if (!productID) return;

    fetch(`https://striveschool-api.herokuapp.com/api/product/${productID}`, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI2NjNkZTkzMTgxODQ0MjAwMTUzNzU5MTYiLCJpYXQiOjE3MTUzMzM0MjUsImV4cCI6MTcxNjU0MzAyNX0.RL2tSvTK9BzLZruwVZmBN0ZpWYeJbBANQWPwJKwcp38"
        }
    })
        .then((response) => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Errore nel recupero del prodotto');
            }
        })
        .then((asset) => {
            document.getElementById('name').value = asset.name;
            document.getElementById('description').value = asset.description;
            document.getElementById('brand').value = asset.brand;
            document.getElementById('price').value = asset.price;
            document.getElementById('imageUrl').value = asset.imageUrl;
            modifyProduct = asset;
        })
        .catch((err) => {
            console.log('ERRORE', err);
        });
};

document.addEventListener('DOMContentLoaded', productData);


if (productID) {
    productData()
    document.getElementsByClassName('btn-primary').innerText = "Modifica"
}
const eliminaProdotto = async function () {
    if (confirm("sei sicuro di voler eliminare il prodotto?")) {
        try {
            if (!productID) {
                throw new Error("ID prodotto non valido");
            }

            const response = await fetch(`https://striveschool-api.herokuapp.com/api/product/${productID}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI2NjNkZTkzMTgxODQ0MjAwMTUzNzU5MTYiLCJpYXQiOjE3MTUzMzM0MjUsImV4cCI6MTcxNjU0MzAyNX0.RL2tSvTK9BzLZruwVZmBN0ZpWYeJbBANQWPwJKwcp38"
                }
            });
            if (!response.ok) {
                throw new Error('Errore nell\'eliminazione del prodotto');
            }

            alert('Prodotto eliminato');
        } catch (err) {
            console.log('ERRORE', err);
        }
    }
};
