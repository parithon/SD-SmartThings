﻿var websocket = null,
    actionInfo = {},
    info = {},
    uuid = null,
    globalSettings = {};

function registerPluginOrPI(inEvent, inUUID) {
    if (websocket) {
        var json = {
            event: inEvent,
            uuid: inUUID
        };
        websocket.send(JSON.stringify(json));
    }
}

function requestGlobalSettings(inUUID) {
    if (websocket) {
        var json = {
            event: "getGlobalSettings",
            context: inUUID
        };
        websocket.send(JSON.stringify(json));
    }
}

function saveGlobalSettings(inUUID) {
    if (websocket) {
        var json = {
            event: "setGlobalSettings",
            context: inUUID,
            payload: globalSettings
        };
        websocket.send(JSON.stringify(json));
    }
}

function connectElgatoStreamDeckSocket(inPort, inUUID, inRegisterEvent, inInfo, inActionInfo) {
    actionInfo = JSON.parse(inActionInfo);
    info = JSON.parse(inInfo);
    uuid = inUUID;

    const sceneSelect = document.getElementById("device-select");
    sceneSelect.addEventListener('change', (evt) => {
        var json = {
            action: actionInfo.action,
            event: "sendToPlugin",
            context: uuid,
            payload: {
                command: "select",
                deviceId: sceneSelect.value
            }
        };
        websocket.send(JSON.stringify(json));
    });

    websocket = new WebSocket(`ws://127.0.0.1:${inPort}`);

    websocket.onopen = () => {
        registerPluginOrPI(inRegisterEvent, inUUID);
        requestGlobalSettings(inUUID);
    }

    websocket.onmessage = (evt) => {
        var jsonObj = JSON.parse(evt.data);
        var event = jsonObj["event"];
        var jsonPayload = jsonObj["payload"];

        if (event === "didReceiveGlobalSettings") {
            globalSettings = jsonPayload["settings"];
            if (!globalSettings.hasOwnProperty("authToken")) {
                window.open("setup.html");
            }
        }

        if (event === "sendToPropertyInspector") {
            console.log(`sendToPropertyInspector: ${JSON.stringify(jsonPayload)}`);
            var scenes = jsonPayload.devices;
            sceneSelect.innerHTML = `<option>Select a device</option>`;
            sceneSelect.innerHTML += scenes.map(s => `<option value="${s.DeviceId}" ${s.Selected ? "selected" : ""}>${s.Label}</option>`);
        }
    }
}

function getCallbackFromWindow(authToken) {
    console.log(`authToken: ${authToken}`);
    if (websocket) {
        globalSettings.authToken = authToken;
        saveGlobalSettings(uuid);
    }
}

function refreshScenes() {
    if (websocket) {
        var json = {
            action: actionInfo.action,
            event: "sendToPlugin",
            context: uuid,
            payload: {
                command: "refresh"
            }
        };
        websocket.send(JSON.stringify(json));
    }
}
