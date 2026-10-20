import { defineStore } from "pinia";
import { computed, ref } from "vue";
import type { Status } from "@/types/Status";
import type { Role } from "@/types/Role";
import type { Credentials } from "@/types/Credentials";
import { authApi } from "@/api/auth";
import type { ExpirationReason } from "@/types/ExpirationReason";

export const useSession = defineStore('session', () => {
    const status = ref<Status>('unknown')
    const login = ref<string | null>(null)
    const role = ref<Role | null>(null)
    const expired = ref<boolean>(false)

    function reset(reason: ExpirationReason = 'manual') {
        status.value = 'guest'
        login.value = null
        role.value = null
        expired.value = reason === 'expired'
    }

    const isAuthed = computed(() => status.value === 'authed')
    const isAdmin = computed(() => role.value === 'Admin')

    let initPromise: Promise<void> | null = null
    function init(): Promise<void> {
        if (status.value !== 'unknown') return Promise.resolve()
        if (!initPromise) { initPromise = getMe() }
        return initPromise
    }

    async function getMe() {
        try {
            const me = await authApi.me()
            login.value = me.login
            role.value = me.role
            status.value = 'authed'
            expired.value = false
        } catch (e) { reset() }
    }

    async function logIn(credentials: Credentials) {
        await authApi.login(credentials)
        await getMe()
    }

    async function register(credentials: Credentials) {
        await authApi.reg(credentials)
        await logIn(credentials)
    }

    async function logOut() {
        try { await authApi.logout() }
        catch { }
        finally { reset() }
    }

    return {
        status, login, role, expired,
        isAuthed, isAdmin,
        init, getMe, logIn, register, logOut, reset,
    }
})