<script setup lang="ts">
import { computed } from 'vue';
import { useSession } from './stores/session';
import router from './router';

const session = useSession()

async function logout() {
    await session.logOut()
    router.push({ name: 'login' })
}
</script>

<template>
    <header>
        <div v-if="session.isAuthed">
            <div>
                <span>Login: {{ session.login }}</span>
                <span>Role: {{ session.role }}</span>
            </div>
            <nav>
                <RouterLink :to="{ name: 'tasks' }">Tasks</RouterLink>
                <RouterLink :to="{ name: 'account' }">Account</RouterLink>
                <RouterLink v-if="session.isAdmin" :to="{ name: 'admin' }">Admin</RouterLink>
            </nav>
            <div>
                <BaseButton @click="logout">Logout</BaseButton>
            </div>
        </div>
        <div v-else>
            <nav>
                <RouterLink :to="{ name: 'login' }">Login</RouterLink>
                <RouterLink :to="{ name: 'reg' }">Sign up</RouterLink>
            </nav>
        </div>
    </header>
    <main><router-view /></main>
</template>

<style scoped></style>
