const uri = '/Task';
let tasks = [];
let token = sessionStorage.getItem("token");

getItems(token);

function getItems() {

    var myHeaders = new Headers();
    myHeaders.append("Authorization", "Bearer " + token);
    myHeaders.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };

    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.log('error', error));
}

function getItemById() {
    const id = document.getElementById('get-item').value;
    var myHeaders = new Headers();
    myHeaders.append("Authorization", "Bearer " + token);
    myHeaders.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    fetch(`${uri}/${id}`, requestOptions)
        .then(response => response.json())
        .then(data => {
            if (data.title == 'Not Found') { alert('Not Found!!'); }
            else { showItem(data); }
        }
        )
        .catch(error => console.error('Unable to get items.', error));
}

function showItem(data) {
    const name = document.getElementById('name');
    const status = document.getElementById('status');
    name.innerText = "TaskName: " + data.descreption;
    status.innerText = "status: " + data.status;

}

function addItem() {
    const addNameTextbox = document.getElementById('add-name').value;
    const item = {
        id: 0,
        descreption: addNameTextbox,
        status: false,
        userId: 123
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        }
    }).then(() => { getItems(token) })
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {

    const item = tasks.find(item => item.id === id);
    document.getElementById('edit-name').value = item.descreption;
    document.getElementById('edit-status').checked = item.status;
    document.getElementById('editForm').style.display = 'block';
    console.log(item);
    saveOnClick(item)
}

function saveOnClick(item){
    document.getElementById('save').onclick = () => {
    const item1 = {
        id: parseInt(item.id),
        status: document.getElementById('edit-status').checked,
        descreption: document.getElementById('edit-name').value.trim()
    };
    console.log(item1);
    updateItem(item1)
}

function updateItem(item1) {
        console.log(item1);
        fetch(`${uri}/${item1.id}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            },
            body: JSON.stringify(item1)
        })
            .then(() => getItems(token))
            .catch(error => console.error('Unable to update item.', error));
        closeInput();
    }
}


function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'task kinds';
    document.getElementById('counter').innerText = `${itemCount} ${name} `;
}

function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let statusCheckbox = document.createElement('input');
        statusCheckbox.type = 'checkbox';
        statusCheckbox.disabled = true;
        statusCheckbox.checked = item.status;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(statusCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.descreption);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;

}

/// user ///
const url = '/user';

function getAllUsers() {
    var myHeaders = new Headers();
    myHeaders.append("Authorization", "Bearer " + token);
    myHeaders.append("Content-Type", "application/json");

    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow',
    };

    fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => { console.log('error', error), alert('Not Authorized!!') });

}

function _displayUsers(data) {
    document.getElementById('manager').style.display = 'block';
    const tBody = document.getElementById('Users');
    tBody.innerHTML = '';
    data.forEach(user => {
        const button = document.createElement('button');

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.userId})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = '❌';
        deleteButton.setAttribute('onclick', `deleteUser(${user.userId})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(user.userId);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(user.username);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(user.password);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

}

let flag = false;
function getMyUser() {
    if (flag)
        return;
    var myHeaders = new Headers();
    myHeaders.append("Authorization", "Bearer " + token);
    myHeaders.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };
    fetch(`${url}/GetMyUser`, requestOptions)
        .then(response => response.json())
        .then(data => {
            if (data.title == 'Not Found') { alert('Not Found!!'); }
            else { showUser(data); }
        }
        ).then(flag = true)
        .catch(error => console.error('Unable to get items.', error));
}

function showUser(data) {
    const id = document.createElement('th');
    const name = document.createElement('th');
    const isAdmin = document.createElement('th');
    const password = document.createElement('th');
    id.innerHTML = data.userId;
    name.innerHTML = data.username;
    isAdmin.innerHTML = data.manager;
    password.innerHTML = data.password;
    const tbl = document.getElementById("tbl");
    tbl.append(id, name, isAdmin, password);
}

function addUser() {
    const addPassword = document.getElementById('add-password').value.trim();
    const addName = document.getElementById('add-user-name').value.trim();
    const user = {
        UserName: addName,
        Password: addPassword
    }
    fetch(`${url}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        },
        body: JSON.stringify(user)
    })
        .then((response) => {
            response.json()
        })
        .then(() => { getAllUsers() })
        .catch(() => alert("Not Authorized!!"));
}

function deleteUser(id) {
    fetch(`${url}/?id=${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
    })
        // .then(() => getAllUsers())
        .then(() => {
            getAllUsers()
            addName.value = ""
            addPassword.value = ""
        }
        )
        .catch(error => console.log('Unable to delete user.', error));
}

function logOut() {
    location.href = "/index.html";
}

