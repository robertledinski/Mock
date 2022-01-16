"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.reducer = exports.actionCreators = void 0;
exports.actionCreators = {
    requestAccount: function () { return function (dispatch, getState) {
        var appState = getState();
        try {
            if (appState && appState.account) {
                fetch("account")
                    .then(function (response) { return response.json(); })
                    .then(function (data) {
                    dispatch({ type: 'RECEIVE_ACCOUNT', account: data });
                });
                dispatch({ type: 'REQUEST_ACCOUNT' });
            }
        }
        catch (e) {
            debugger;
            throw e;
        }
    }; }
};
var unloadedState = { isLoading: false, account: { totalCredits: 0, totalDebits: 0, endOfDayBalances: [] } };
var reducer = function (state, incomingAction) {
    if (state === undefined) {
        return unloadedState;
    }
    var action = incomingAction;
    switch (action.type) {
        case 'REQUEST_ACCOUNT':
            return {
                account: state.account,
                isLoading: true
            };
        case 'RECEIVE_ACCOUNT':
            return {
                account: action.account,
                isLoading: false
            };
        default:
            return state;
    }
};
exports.reducer = reducer;
//# sourceMappingURL=AccountReducer.js.map