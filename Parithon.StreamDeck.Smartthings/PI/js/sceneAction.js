﻿var websocket = null,
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
    var actionInfo = JSON.parse(inActionInfo);
    var info = JSON.parse(inInfo);

    const deviceSelect = document.getElementById("device-select");

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
        }

        if (event === "sendToPropertyInspector") {
            if (jsonPayload.hasOwnProperty("Devices")) {
                var devices = jsonPayload["Devices"];
                deviceSelect.innerHtml = "";
                devices.map(device => {
                    const option = new HTMLOptionElement();
                    option.value = device.deviceId;
                    option.text = device.Label;
                    option.selected = jsonPayload["SelectedDevice"] && jsonPayload["SelectedDevice"] === device.deviceId;
                    deviceSelect.appendChild(option);
                });
            }
        }
    }
}