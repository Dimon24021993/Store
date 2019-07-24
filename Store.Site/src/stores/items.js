import Vapi from "vuex-rest-api";

const api = new Vapi({
    state: {
        pagination: {
            entities: [],
            page: 1,
            size: 12,
            asc: true
        },
        item: {}
    }
}).post({
    action: "getItems",
    property: "pagination",
    path: "/Items/GetItemsPagination"
}).get({
    action: "getItem",
    property: "item",
    queryParams: true,
    path: "/Items/GetItemByNo"
}).getStore();


export default {
    namespaced: true,
    state: api.state,
    getters: api.getters,
    mutations: api.mutations,
    actions: api.actions
};